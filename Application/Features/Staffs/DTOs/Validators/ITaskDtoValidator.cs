using Application.Features.Staffs.DTOs;
using FluentValidation;
using static Domain.Staff;
using Domain;

namespace HFC.Application.Features.Staffs.DTOs.Validators
{
    public class IStaffDtoValidator : AbstractValidator<IStaffDto>
    {
        public IStaffDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

            RuleFor(p => p.About)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");
            RuleFor(p => p.UserSector)
                .Must(role => Enum.TryParse(typeof(Staff.Sector), role, out _))
                .WithMessage("Invalid {PropertyName} value!");
        }


    }
}
