using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Persistence;
using Microsoft.EntityFrameworkCore;
using static Domain.AppUser;

namespace Infrastructure.Security
{
    public class IsSuperAdminRequirement : IAuthorizationRequirement
    {

    }

    public class IsSuperAdminRequirementHandler : AuthorizationHandler<IsSuperAdminRequirement>
    {
        private readonly DataContext _dbcontext;
        public IsSuperAdminRequirementHandler(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsSuperAdminRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Task.CompletedTask;


            var user = _dbcontext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId).Result;

            if (user == null) return Task.CompletedTask;

            if (user.UserRole == Role.SuperAdmin) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}