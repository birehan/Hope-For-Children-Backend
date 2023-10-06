using Application.Features.Projects.DTOs;
using FluentValidation;

namespace HFC.Application.Features.Projects.DTOs.Validators
{
    public class IProjectDtoValidator : AbstractValidator<IProjectDto>
    {
        public IProjectDtoValidator()
        {


            RuleFor(p => p.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(1500)
                .WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");
        }


    }
}
