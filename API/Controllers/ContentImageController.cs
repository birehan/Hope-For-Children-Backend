
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPhotoAccessor _photoAccessor;

        public ContentImageController(IWebHostEnvironment webHostEnvironment, IPhotoAccessor photoAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _photoAccessor = photoAccessor;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            try
            {
                Console.WriteLine("Upload Called");

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file selected for upload.");
                }

                // Use the PhotoAccessor to upload the image to Cloudinary
                var uploadResult = await _photoAccessor.AddPhoto(file);

                if (uploadResult == null)
                {
                    return StatusCode(500, "Failed to upload image.");
                }
                Console.WriteLine("success upload");
                return Ok(new { imageUrl = uploadResult.Url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
