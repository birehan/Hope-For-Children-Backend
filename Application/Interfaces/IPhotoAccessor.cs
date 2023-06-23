using Microsoft.AspNetCore.Http;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IPhotoAccessor
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file);

        Task<PhotoUploadResult> UpdatePhoto(IFormFile file, string publicId);


        Task<string> DeletePhoto(string publicId);

    }
}