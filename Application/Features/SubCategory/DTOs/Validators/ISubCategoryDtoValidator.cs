using FluentValidation;

namespace Application.Features.SubCategories.DTOs.Validators
{
    public class ISubCategoryDtoValidator : AbstractValidator<ISubCategoryDto>
    {
        public ISubCategoryDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{Property Name} is required")
                .NotNull();
        }
    }
}
