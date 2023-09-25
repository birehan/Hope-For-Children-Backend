using Application.Contracts.Persistence;
using Application.Features.Categories.DTOs;
using Application.Features.Categories.DTOs.Validators;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.CQRS.Commands
{
    public class CreateCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public GalleryDto GalleryDto { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
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

        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {



            var items = new CreateCategoryDto { Title = request.GalleryDto.Title };

            for (var index = 0; index < request.GalleryDto.Photos.Count; index++)
            {
                var galleryPhoto = new GalleryPhotoDto
                {
                    File = request.GalleryDto.Photos[index],
                    IsMainPhoto = index == request.GalleryDto.MainPhotoIndex
                };

                items.Photos.Add(galleryPhoto);
            }



            var validator = new CreateCategoryDtoValidator();
            var validationResult = await validator.ValidateAsync(items);


            if (!validationResult.IsValid)
                return Result<CategoryDto>.Failure(validationResult.Errors[0].ErrorMessage);

            var category = _mapper.Map<CreateCategoryDto, Category>(items);

            if (items.Photos != null && items.Photos.Any())
            {
                foreach (var photoDto in items.Photos)
                {
                    var photoUploadResult = await _photoAccessor.AddPhoto(photoDto.File);

                    if (photoUploadResult == null)
                    {
                        return Result<CategoryDto>.Failure("Error uploading one or more photos");
                    }

                    var galleryPhoto = new GalleryPhoto
                    {
                        Url = photoUploadResult.Url,
                        Id = photoUploadResult.PublicId,
                        IsMainPhoto = photoDto.IsMainPhoto // Set IsMainPhoto based on the value in the DTO
                    };

                    category.Photos.Add(galleryPhoto);

                    // If the current photo is the main photo, update the main photo URL in the category
                    if (galleryPhoto.IsMainPhoto)
                    {
                        category.MainPhotoUrl = galleryPhoto.Url;
                    }
                }
            }

            await _unitOfWork.CategoryRepository.Add(category);

            if (await _unitOfWork.Save() > 0)
            {
                return Result<CategoryDto>.Success(_mapper.Map<CategoryDto>(category));
            }

            return Result<CategoryDto>.Failure("Error while saving changes");
        }
    }
}
