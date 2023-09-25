using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Application.Features.Categories.DTOs;
using static System.String;

namespace Application.Features.Categories.DTOs.Validators
{
    public class ICategoryDtoValidator : AbstractValidator<ICategoryDto>
    {
        public ICategoryDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{Property Name} is required")
                .NotNull();

            RuleForEach(p => p.Photos)
                .Must(photo => !IsNullOrWhiteSpace(photo.PhotoUrl) || photo.File != null)
                .WithMessage("Each photo must have either a File or a PhotoUrl.");

            RuleFor(p => p.Photos)
                .Must(photos => photos.Count(photo => photo.IsMainPhoto) == 1)
                .WithMessage("Exactly one photo must have IsMainPhoto set to true.");



            // RuleForEach(p => p.Photos)
            //     .Must((photo, file) =>
            //     {
            //         // Validate the image format if 'File' is provided
            //         if (file.File != null)
            //         {
            //             return IsValidImageFile(file.File);
            //         }

            //         // Validate that 'PhotoUrl' is not empty if 'File' is not provided
            //         return !string.IsNullOrWhiteSpace(file.PhotoUrl);
            //     })
            //     .WithMessage("Invalid image file format. Allowed formats are .jpg, .jpeg, .png, .gif.");


        }


        private bool IsValidImageFile(IFormFile file)
        {
            if (file == null)
                return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var fileExtension = System.IO.Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }

        // private bool BeValidFile(IFormFile file)
        // {
        //     if (file == null)
        //         return false;

        //     var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        //     var extension = Path.GetExtension(file.FileName);
        //     return validExtensions.Contains(extension.ToLower());
        // }


    }
}
