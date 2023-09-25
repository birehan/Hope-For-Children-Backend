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
    public class UpdateCategoryCommand : IRequest<Result<CategoryDetailDto>>
    {
        public UpdateGalleryDto GalleryDto { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryDetailDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoAccessor _photoAccessor;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<CategoryDetailDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {


            var existingCategory = await _unitOfWork.CategoryRepository.GetCategoryWithPhotos(request.GalleryDto.Id);

            if (existingCategory == null)
                return Result<CategoryDetailDto>.Failure("Category not found");


            var items = new UpdateCategoryDto { Title = request.GalleryDto.Title, Id = request.GalleryDto.Id };

            for (var index = 0; index < request.GalleryDto.Photos.Count; index++)
            {
                var galleryPhoto = new GalleryPhotoDto
                {
                    File = request.GalleryDto.Photos[index],
                    IsMainPhoto = index == request.GalleryDto.MainPhotoIndex
                };

                items.Photos.Add(galleryPhoto);
            }

            for (var index = 0; index < request.GalleryDto.OldPhotos.Count; index++)
            {

                var galleryPhoto = new GalleryPhotoDto
                {
                    PhotoUrl = request.GalleryDto.OldPhotos[index],
                    IsMainPhoto = index == request.GalleryDto.OldMainPhotoIndex
                };

                items.Photos.Add(galleryPhoto);
            }

            var validator = new UpdateCategoryDtoValidator();
            var validationResult = await validator.ValidateAsync(items);


            if (!validationResult.IsValid)
                return Result<CategoryDetailDto>.Failure(validationResult.Errors[0].ErrorMessage);

            var category = _mapper.Map<UpdateCategoryDto, Category>(items);
            var newPhotos = new List<GalleryPhoto>();

            if (items.Photos != null && items.Photos.Any())
            {
                foreach (var photoDto in items.Photos)
                {
                    if (photoDto.File != null)
                    {
                        var photoUploadResult = await _photoAccessor.AddPhoto(photoDto.File);

                        if (photoUploadResult == null)
                        {
                            return Result<CategoryDetailDto>.Failure("Error uploading one or more photos");
                        }

                        var galleryPhoto = new GalleryPhoto
                        {
                            Url = photoUploadResult.Url,
                            Id = photoUploadResult.PublicId,
                            IsMainPhoto = photoDto.IsMainPhoto // Set IsMainPhoto based on the value in the DTO
                        };

                        newPhotos.Add(galleryPhoto);

                        if (galleryPhoto.IsMainPhoto)
                        {
                            category.MainPhotoUrl = galleryPhoto.Url;
                        }
                    }
                    else
                    {
                        var existingPhoto = existingCategory.Photos.FirstOrDefault(x => x.Url == photoDto.PhotoUrl);
                        if (existingPhoto != null)
                        {
                            existingPhoto.IsMainPhoto = photoDto.IsMainPhoto;
                            if (existingPhoto.IsMainPhoto)
                            {
                                category.MainPhotoUrl = existingPhoto.Url;
                            }
                            newPhotos.Add(existingPhoto);
                        }

                    }
                }
            }

            foreach (var oldPhoto in existingCategory.Photos)
            {
                var oldPhotoUrl = oldPhoto.Url;

                if (!newPhotos.Any(newPhoto => newPhoto.Url == oldPhotoUrl))
                {
                    await _photoAccessor.DeletePhoto(oldPhoto.Id);
                }
            }

            existingCategory.Title = request.GalleryDto.Title;
            existingCategory.Photos = newPhotos;
            await _unitOfWork.CategoryRepository.Update(existingCategory);


            if (await _unitOfWork.Save() > 0)
            {
                return Result<CategoryDetailDto>.Success(_mapper.Map<CategoryDetailDto>(existingCategory));
            }

            return Result<CategoryDetailDto>.Failure("Error while saving changes");
        }

    }
}
