using MediatR;
using Application.Responses;
using Domain;
using Application.Features.Accounts.DTOs;
using Microsoft.AspNetCore.Identity;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Features.Accounts.CQRS.Queries

{
    public class GetCurrentUserQuery : IRequest<Result<UserDto>>
    {
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly TokenService _tokenService;

        private readonly IUserAccessor _userAccessor;


        public GetCurrentUserQueryHandler(UserManager<AppUser> userManager, TokenService token, IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _tokenService = token;
            _userAccessor = userAccessor;
        }

        public async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken: cancellationToken);

            if (user == null) return Result<UserDto>.Failure("Unauthorized");

            var response = new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email,
                UserRole = user.UserRole.ToString()
            };
            return Result<UserDto>.Success(response);

        }
    }

}
