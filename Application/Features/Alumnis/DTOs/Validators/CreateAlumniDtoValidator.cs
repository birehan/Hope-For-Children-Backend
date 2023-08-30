using Application.Features.Alumnis.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace HFC.Application.Features.Alumnis.DTOs.Validators
{
    public class CreateAlumniDtoValidator : AbstractValidator<CreateAlumniDto>
    {
        public CreateAlumniDtoValidator()
        {
            Include(new IAlumniDtoValidator());

            RuleFor(p => p.File)
                .Must(BeAValidImage)
                .WithMessage("{PropertyName} must be a valid image file.");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null)
            {
                return true; // Skip validation if file is not provided
            }

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            var extension = Path.GetExtension(file.FileName);
            return validExtensions.Contains(extension.ToLower());
        }
    }
}