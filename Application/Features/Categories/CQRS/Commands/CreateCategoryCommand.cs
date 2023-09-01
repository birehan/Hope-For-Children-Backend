using Application.Contracts.Persistence;
using Application.Features.Categories.DTOs;
using Application.Features.Categories.DTOs.Validators;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Categories.CQRS.Commands
{
    public class CreateCategoryCommand : IRequest<Result<Guid>>
    {
        public CreateCategoryDto CategoryDto { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoAccessor _photoAccessor;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {



            var validator = new CreateCategoryDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CategoryDto);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);


            var category = _mapper.Map<CreateCategoryDto, Category>(request.CategoryDto);

            var mainPhotoResult = await _photoAccessor.AddPhoto(request.CategoryDto.MainPhoto);
            if (mainPhotoResult == null)
            {
                return Result<Guid>.Failure("Error uploading main photo");
            }

            category.MainPhotoUrl = mainPhotoResult.Url;
            category.Photos.Add(new Photo { Url = mainPhotoResult.Url, Id = mainPhotoResult.PublicId });

            if (request.CategoryDto.Photos != null && request.CategoryDto.Photos.Count > 0)
            {
                var photoUploadTasks = request.CategoryDto.Photos.Select(p => _photoAccessor.AddPhoto(p));
                var photoUploadResults = await Task.WhenAll(photoUploadTasks);

                if (photoUploadResults.Any(r => r == null))
                {
                    return Result<Guid>.Failure("Error uploading one or more photos");
                }

                category.Photos.AddRange(photoUploadResults.Select(r => new Photo { Url = r.Url, Id = r.PublicId }));
            }

            await _unitOfWork.CategoryRepository.Add(category);

            if (await _unitOfWork.Save() > 0)
            {
                return Result<Guid>.Success(category.Id);
            }

            return Result<Guid>.Failure("Error while saving changes");
        }
    }
}
