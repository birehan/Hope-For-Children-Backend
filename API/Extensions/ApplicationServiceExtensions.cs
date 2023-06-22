using System.Net.Mime;
using Application.Contracts.Infrastructure;
using Application.Interfaces;
using Application.Models;
using Application.Responses;
using Infrastructure.Mail;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Keys
                        .SelectMany(key => context.ModelState[key].Errors.Select(x => $"{x.ErrorMessage}"))
                        .ToArray();

                    var apiError = Result<string>.Failure(errors[0]);
                    var result = new BadRequestObjectResult(apiError); // Return BadRequestObjectResult instead of ObjectResult
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);

                    return result;
                };
            });


            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();

            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));


            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    policy =>
                        policy
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .WithOrigins("http://localhost:3000"));
            });



            return services;
        }
    }
}