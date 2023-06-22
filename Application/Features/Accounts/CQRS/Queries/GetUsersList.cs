
using MediatR;
using Application.Responses;
using Domain;
using Application.Features.Accounts.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.Features.Accounts.CQRS.Queries

{
    public class GetUsersListQuery : IRequest<Result<List<UserAccountDto>>>
    {
    }

    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, Result<List<UserAccountDto>>>
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IMapper _mapper;


        public GetUsersListQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task<Result<List<UserAccountDto>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);
            return Result<List<UserAccountDto>>.Success(_mapper.Map<List<UserAccountDto>>(users));
        }
    }

}
