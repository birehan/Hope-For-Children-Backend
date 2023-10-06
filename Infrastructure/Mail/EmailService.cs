using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Application.Models;
using Application.Responses;

namespace Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail
                ?? throw new ArgumentNullException(nameof(fluentEmail));
        }


        public async Task<Result<string>> Send(EmailMetadata emailMetadata)
        {
            try
            {
                await _fluentEmail.To(emailMetadata.ToAddress)
                    .Subject(emailMetadata.Subject)
                    .Body(emailMetadata.Body)
                    .SendAsync();

                return Result<string>.Success("Email sent successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Email sending failed: {ex.Message}");
            }
        }
    }
}