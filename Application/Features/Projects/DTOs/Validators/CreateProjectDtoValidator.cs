using Application.Features.Projects.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace HFC.Application.Features.Projects.DTOs.Validators
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            Include(new IProjectDtoValidator());

            RuleFor(p => p.ImageFile)
                .Must(BeAValidImage)
                .WithMessage("{PropertyName} must be a valid image file.");

            RuleFor(p => p.PdfFile)
               .Must(BeAValidFile)
               .WithMessage("{PropertyName} must be a valid pdf file and size of less than 3 mega bite.");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null)
            {
                return true; // Skip validation if file is not provided
            }

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var extension = Path.GetExtension(file.FileName);
            return validExtensions.Contains(extension.ToLower());
        }

        private bool BeAValidFile(IFormFile file)
        {
            if (file == null)
            {
                return true; // Skip validation if file is not provided
            }

            var validExtensions = new[] { ".pdf" };
            var validMaxSize = 3 * 1024 * 1024; // 3MB in bytes

            var extension = Path.GetExtension(file.FileName);
            var fileSize = file.Length;

            return validExtensions.Contains(extension.ToLower()) && fileSize <= validMaxSize;
        }

    }
}