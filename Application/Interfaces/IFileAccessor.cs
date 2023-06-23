using Application.Responses;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFileAccessor
    {
        Task<FileUploadResult> UploadFile(IFormFile file);

        Task<FileUploadResult> UpdateFile(IFormFile file, string existingPublicId);

        Task<string> DeleteFile(string publicId);
    }
}
