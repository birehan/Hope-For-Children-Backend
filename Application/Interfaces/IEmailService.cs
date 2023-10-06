using Application.Models;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task<Result<string>> Send(EmailMetadata emailMetadata);

    }
}