
using Application.Features.Staffs.DTOs;
using FluentValidation;

namespace HFC.Application.Features.Staffs.DTOs.Validators
{
    public class UpdateStaffDtoValidator : AbstractValidator<UpdateStaffDto>
    {
        public UpdateStaffDtoValidator()
        {
            Include(new IStaffDtoValidator());

            RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");

        }
    }
}