using MediatR;
using Application.Responses;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Contracts.Infrastructure;
using Application.Models;

namespace Application.Features.Accounts.CQRS.Commands
{
    public class ForgetPasswordSendEmailCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
    }

    public class ForgetPasswordSendEmailCommandHandler : IRequestHandler<ForgetPasswordSendEmailCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender; // Inject the IEmailSender interface

        public ForgetPasswordSendEmailCommandHandler(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<Result<string>> Handle(ForgetPasswordSendEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("User does not exist!");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // var token = await _userManager.GeneratePasswordResetTokenAsync(user, "Default");


            // Create the password reset URL
            var resetUrl = "https://example.com/resetpassword?userId=" + user.Id + "&token=" + token;

            // Compose the email
            var email = new Email
            {
                To = user.Email,
                Subject = "Password Reset",
                Body = $"Please reset your password by clicking the following link: {resetUrl}"
            };

            var isEmailSent = await _emailSender.SendEmail(email);

            if (!isEmailSent)
            {
                return Result<string>.Failure("Failed to send password reset email!");
            }

            return Result<string>.Success("Password reset email sent successfully!");
        }
    }
}
