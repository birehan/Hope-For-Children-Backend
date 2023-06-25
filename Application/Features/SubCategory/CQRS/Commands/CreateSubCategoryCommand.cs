using Application.Contracts.Persistence;
using Application.Features.SubCategories.DTOs;
using Application.Features.SubCategories.DTOs.Validators;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;


namespace Application.Features.SubCategories.CQRS.Commands
{
    public class CreateSubCategoryCommand : IRequest<Result<Guid>>
    {
        public CreateSubCategoryDto SubCategoryDto { get; set; }
    }

    public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommand, Result<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoAccessor _photoAccessor;

        public CreateSubCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Guid>> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateCategoryDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SubCategoryDto, CancellationToken.None);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure("Validation Failure");

            var existingSubCategory = await _unitOfWork.SubCategoryRepository.GetByTitleAsync(request.SubCategoryDto.Title);

            if (existingSubCategory != null)
            {
                var mainPhotoResult = await _photoAccessor.AddPhoto(request.SubCategoryDto.MainPhoto);
                if (mainPhotoResult == null)
                {
                    return Result<Guid>.Failure("Error uploading main photo");
                }

                existingSubCategory.MainPhoto = new Photo
                {
                    Url = mainPhotoResult.Url,
                    Id = mainPhotoResult.PublicId,
                };

                var photoUploadTasks = request.SubCategoryDto.Photos.Select(p => _photoAccessor.AddPhoto(p));
                var photoUploadResults = await Task.WhenAll(photoUploadTasks);

                if (photoUploadResults.Any(r => r == null))
                {
                    return Result<Guid>.Failure("Error uploading one or more photos");
                }

                existingSubCategory.Photos.AddRange(photoUploadResults.Select(r => new Photo { Url = r.Url, Id = r.PublicId, }));


                await _unitOfWork.SubCategoryRepository.Update(existingSubCategory);

                if (await _unitOfWork.Save() > 0)
                {
                    return Result<Guid>.Success(existingSubCategory.Id);
                }

                return Result<Guid>.Failure("Error while saving changes");
            }
            else
            {
                var existingCategory = await _unitOfWork.CategoryRepository.GetByTitleAsync(request.SubCategoryDto.CategoryTitle);
                var categoryId = new Guid();

                if (existingCategory != null)
                {
                    categoryId = existingCategory.Id;
                }
                else
                {
                    var category = new Category { Title = request.SubCategoryDto.CategoryTitle };
                    await _unitOfWork.CategoryRepository.Add(category);
                    categoryId = category.Id;
                }

                var subCategory = _mapper.Map<SubCategory>(request.SubCategoryDto);
                subCategory.CategoryId = categoryId;

                var mainPhotoResult = await _photoAccessor.AddPhoto(request.SubCategoryDto.MainPhoto);
                if (mainPhotoResult == null)
                {
                    return Result<Guid>.Failure("Error uploading main photo");
                }

                subCategory.MainPhoto = new Photo
                {
                    Url = mainPhotoResult.Url,
                    Id = mainPhotoResult.PublicId,
                };

                if (request.SubCategoryDto.Photos != null && request.SubCategoryDto.Photos.Count > 0)
                {
                    var photoUploadTasks = request.SubCategoryDto.Photos.Select(p => _photoAccessor.AddPhoto(p));
                    var photoUploadResults = await Task.WhenAll(photoUploadTasks);

                    if (photoUploadResults.Any(r => r == null))
                    {
                        return Result<Guid>.Failure("Error uploading one or more photos");
                    }

                    subCategory.Photos = photoUploadResults.Select(r => new Photo { Url = r.Url, Id = r.PublicId, }).ToList();
                }

                await _unitOfWork.SubCategoryRepository.Add(subCategory);

                if (await _unitOfWork.Save() > 0)
                {
                    return Result<Guid>.Success(subCategory.Id);
                }

                return Result<Guid>.Failure("Error while saving changes");
            }
        }
    }
}
