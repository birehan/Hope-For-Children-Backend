using Application.Features.Staffs.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace HFC.Application.Features.Staffs.DTOs.Validators
{
    public class CreateStaffDtoValidator : AbstractValidator<CreateStaffDto>
    {
        public CreateStaffDtoValidator()
        {
            Include(new IStaffDtoValidator());

            RuleFor(p => p.File)
                    .Must(BeAValidImage).WithMessage("{PropertyName} must be a valid image file.");
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