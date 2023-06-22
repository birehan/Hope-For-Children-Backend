using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Accounts.DTOs.Validators
{

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private readonly UserManager<AppUser> _userManager;

        public CreateUserDtoValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            RuleFor(p => p.UserName)
                .MustAsync(async (userName, token) =>
                {
                    var existingUser = await _userManager.FindByNameAsync(userName);
                    return existingUser == null;
                })
                .WithMessage("{PropertyName} taken!");


            RuleFor(p => p.Email)
                .MustAsync(async (email, token) =>
                {
                    var existingUser = await _userManager.FindByEmailAsync(email);

                    return existingUser == null;
                })
                .WithMessage("{PropertyName} taken!");

            RuleFor(p => p.UserRole)
                .Must(role => Enum.TryParse(typeof(AppUser.Role), role, out _))
                .WithMessage("Invalid {PropertyName} value!");

        }
    }
}