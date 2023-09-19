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

using System.Text.Json;
using System.Collections.Generic;

namespace Application.Features.Categories.CQRS.Commands
{
    public class CreateCategoryCommand : IRequest<Result<Guid>>
    {
        public GalleryDto GalleryDto { get; set; }
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

            List<GalleryPhotoDto> galleryPhotos = null;

                try
                {
                    galleryPhotos = JsonSerializer.Deserialize<List<GalleryPhotoDto>>(request.GalleryDto.Photos);

                    // You now have your data in the 'galleryPhotos' variable.
                    foreach (var photo in galleryPhotos)
                    {
                        Console.WriteLine($"IsMainPhoto: {photo.IsMainPhoto}, PhotoUrl: {photo.PhotoUrl}");
                    }
                }
                catch (JsonException e)
                {
                    // Handle any JSON parsing errors here.
                    Console.WriteLine($"JSON parsing error: {e.Message}");
                }


            var categoryDto = new CreateCategoryDto
            {
                Title = request.GalleryDto.Title,
                Photos = galleryPhotos
            };

            
            var validator = new CreateCategoryDtoValidator();
            var validationResult = await validator.ValidateAsync(categoryDto);


            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);

            var category = _mapper.Map<CreateCategoryDto, Category>(categoryDto);

            if (categoryDto.Photos != null && categoryDto.Photos.Any())
            {
                foreach (var photoDto in categoryDto.Photos)
                {
                    var photoUploadResult = await _photoAccessor.AddPhoto(photoDto.File);

                    if (photoUploadResult == null)
                    {
                        return Result<Guid>.Failure("Error uploading one or more photos");
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
                return Result<Guid>.Success(category.Id);
            }

            return Result<Guid>.Failure("Error while saving changes");
        }
    }
}
