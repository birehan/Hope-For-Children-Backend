using Application.Features.Accounts.DTOs;
using Application.Responses;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Accounts.CQRS.Commands
{
    public class ResetUserPasswordCommand : IRequest<Result<string>>
    {
        public ResetUserPasswordDto ResetUserPasswordDto { get; set; }
    }

    public class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetUserPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.ResetUserPasswordDto.UserId);

            if (user == null)
            {
                return Result<string>.Failure("User not found!");
            }

            var resetResult = await _userManager.ResetPasswordAsync(
                user,
                request.ResetUserPasswordDto.Token,
                request.ResetUserPasswordDto.NewPassword);

            if (resetResult.Succeeded)
            {
                return Result<string>.Success("Password reset successfully!");
            }

            var errors = resetResult.Errors.Select(e => e.Description).ToList();
            return Result<string>.Failure("Password reset failed: " + string.Join(", ", errors));
        }
    }
}
