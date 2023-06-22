using System.Text;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions
{
    public static class IdentityServerExtensions
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsSuperAdmin", policy =>
                {
                    policy.Requirements.Add(new IsSuperAdminRequirement());
                });
            });

            services.AddTransient<IAuthorizationHandler, IsSuperAdminRequirementHandler>();
            services.AddScoped<TokenService>();

            return services;
        }
    }
}
