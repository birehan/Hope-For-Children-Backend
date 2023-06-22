using MediatR;
using Application.Responses;
using Domain;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Application.Features.Accounts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.CQRS.Commands
{
    public class UpdateUserPasswordCommand : IRequest<Result<string>>
    {
        public UpdateUserPasswordDto UpdateUserPasswordDto { get; set; }
    }
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IUserAccessor _userAccessor;


        public UpdateUserPasswordCommandHandler(UserManager<AppUser> userManager, IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
        }

        public async Task<Result<string>> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken: cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("Unauthorized");
            }

            // Verify if the old password matches the user's current password
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.UpdateUserPasswordDto.OldPassword);
            if (!passwordValid)
            {
                return Result<string>.Failure("Invalid old password");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.UpdateUserPasswordDto.OldPassword, request.UpdateUserPasswordDto.NewPassword);

            if (changePasswordResult.Succeeded)
            {
                return Result<string>.Success("Password updated successfully.");
            }

            var errorMessage = string.Join(", ", changePasswordResult.Errors.Select(e => e.Description));
            return Result<string>.Failure(errorMessage);


        }
    }
}
