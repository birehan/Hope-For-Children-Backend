using MediatR;
using Application.Responses;
using Domain;
using Application.Features.Accounts.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Services;

namespace Application.Features.Accounts.CQRS.Queries

{
    public class LoginQuery : IRequest<Result<UserDto>>
    {
        public LoginDto LoginDto { get; set; }
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly TokenService _tokenService;

        public LoginQueryHandler(UserManager<AppUser> userManager, TokenService token)
        {
            _userManager = userManager;
            _tokenService = token;
        }

        public async Task<Result<UserDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {


            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == request.LoginDto.Email, cancellationToken: cancellationToken);

            if (user == null)
            {
                return Result<UserDto>.Failure("Invalid Email or password");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.LoginDto.Password);
            if (result)
            {
                var response = new UserDto
                {
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user),
                    Email = user.Email,
                    UserRole = user.UserRole.ToString()
                };
                return Result<UserDto>.Success(response);
            }
            return Result<UserDto>.Failure("Invalid Email or password");

        }
    }

}
