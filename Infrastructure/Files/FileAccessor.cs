using Application.Interfaces;
using Application.Responses;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.Photos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Files
{
    public class FileAccessor : IFileAccessor
    {
        private readonly Cloudinary _cloudinary;

        public FileAccessor(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<FileUploadResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    await using var stream = file.OpenReadStream();
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    if (uploadResult.Error != null)
                    {
                        return null;
                    }

                    return new FileUploadResult
                    {
                        PublicId = uploadResult.PublicId,
                        Url = uploadResult.SecureUrl.ToString()
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> DeleteFile(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.Result == "ok" ? result.Result : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<FileUploadResult> UpdateFile(IFormFile file, string existingPublicId)
        {
            try
            {
                if (!string.IsNullOrEmpty(existingPublicId))
                {
                    await DeleteFile(existingPublicId);
                }

                return await UploadFile(file);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
