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

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var extension = Path.GetExtension(file.FileName);
            return validExtensions.Contains(extension.ToLower());
        }


    }
}