using MediatR;
using Application.Responses;
using Domain;
using Microsoft.AspNetCore.Identity;
using Application.Features.Accounts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.CQRS.Commands
{
    public class UpdateUserRoleCommand : IRequest<Result<string>>
    {
        public UpdateUserRoleDto UpdateUserRoleDto { get; set; }
    }

    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public UpdateUserRoleCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.UpdateUserRoleDto.Email, cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("User does not exist!");
            }

            var userRole = request.UpdateUserRoleDto.UserRole;

            if (!Enum.TryParse(userRole, out AppUser.Role role))
            {
                return Result<string>.Failure("Invalid role");
            }

            user.UserRole = request.UpdateUserRoleDto.UserRole == nameof(AppUser.Role.SuperAdmin)
                ? AppUser.Role.SuperAdmin
                : AppUser.Role.Admin;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Result<string>.Success("Update User Role Success.");
            }

            return Result<string>.Failure(result?.Errors.ToList()[0].ToString());
        }
    }
}
