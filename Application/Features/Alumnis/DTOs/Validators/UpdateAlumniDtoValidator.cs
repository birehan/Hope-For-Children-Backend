
using Application.Features.Alumnis.DTOs;
using FluentValidation;

namespace HFC.Application.Features.Alumnis.DTOs.Validators
{
    public class UpdateAlumniDtoValidator : AbstractValidator<UpdateAlumniDto>
    {
        public UpdateAlumniDtoValidator()
        {
            Include(new IAlumniDtoValidator());
            RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");
        }
    }
}