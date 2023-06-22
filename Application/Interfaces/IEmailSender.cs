using Application.Models;

namespace Application.Contracts.Infrastructure
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(Email email);
    }
}
