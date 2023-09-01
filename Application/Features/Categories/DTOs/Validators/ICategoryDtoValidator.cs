using FluentValidation;

namespace Application.Features.Categories.DTOs.Validators
{
    public class ICategoryDtoValidator : AbstractValidator<ICategoryDto>
    {
        public ICategoryDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{Property Name} is required")
                .NotNull();
        }
    }
}
