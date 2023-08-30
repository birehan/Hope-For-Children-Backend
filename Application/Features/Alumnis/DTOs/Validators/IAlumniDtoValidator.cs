using Application.Features.Alumnis.DTOs;
using FluentValidation;

namespace HFC.Application.Features.Alumnis.DTOs.Validators
{
    public class IAlumniDtoValidator : AbstractValidator<IAlumniDto>
    {
        public IAlumniDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

            RuleFor(p => p.Story)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(800)
                .WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");
        }


    }
}
