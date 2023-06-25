using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Features.SubCategories.DTOs.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateSubCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            Include(new ISubCategoryDtoValidator());
            RuleFor(p => p.CategoryTitle)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.MainPhoto)
                .Must(BeValidFile).WithMessage("{PropertyName} must be a valid File file");

            RuleFor(p => p.Photos)
                .Must(BeValidFiles).WithMessage("{PropertyName} must be a valid images");
        }

        private bool BeValidFile(IFormFile file)
        {
            if (file == null)
                return false;

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var extension = Path.GetExtension(file.FileName);
            return validExtensions.Contains(extension.ToLower());
        }

        private bool BeValidFiles(List<IFormFile> files)
        {
            foreach(var file in files)
            {
                if (!BeValidFile(file))
                    return false;
            }

            return true;
        }
    }
}
