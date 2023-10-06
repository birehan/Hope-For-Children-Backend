using MediatR;
using Application.Responses;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Models;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;


namespace Application.Features.Accounts.CQRS.Commands
{
    public class ForgetPasswordSendEmailCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
    }

    public class ForgetPasswordSendEmailCommandHandler : IRequestHandler<ForgetPasswordSendEmailCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService; // Inject the IEmailSender interface

        private readonly IConfiguration _configuration; // Inject IConfiguration


        public ForgetPasswordSendEmailCommandHandler(UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService
            ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<Result<string>> Handle(ForgetPasswordSendEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("User does not exist!");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var clientUrl = _configuration["ClientUrl"];
            var resetUrl = $"{clientUrl}/resetpassword?userId={user.Id}&token={token}";



            EmailMetadata emailMetadata = new(user.Email,
              "Reset Password",
               $"Please reset your password by clicking the following link: {resetUrl}");
            return await _emailService.Send(emailMetadata);
        }
    }
}
