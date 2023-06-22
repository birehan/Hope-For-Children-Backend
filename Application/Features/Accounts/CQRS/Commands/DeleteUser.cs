using MediatR;
using Application.Responses;
using Domain;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.CQRS.Commands
{
    public class DeleteUserCommand : IRequest<Result<string>>
    {
        public string UserName { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserAccessor _userAccessor;

        public DeleteUserCommandHandler(UserManager<AppUser> userManager, IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
        }

        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken: cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("User not found!");
            }


            if (user.UserName == _userAccessor.GetUsername())
            {
                return Result<string>.Failure("User can not delete self account!");
            }

            var result = await _userManager.DeleteAsync(user);


            if (result.Succeeded)
            {
                return Result<string>.Success("Delete User Success.");
            }

            return Result<string>.Failure(result?.Errors.ToList()[0].ToString());
        }
    }
}
