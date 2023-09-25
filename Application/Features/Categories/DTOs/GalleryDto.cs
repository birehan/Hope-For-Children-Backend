using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.DTOs
{
    public class GalleryDto
    {
        public string Title { get; set; }
        public int MainPhotoIndex { get; set; } = -1;
        public List<IFormFile> Photos { get; set; }

    }
}
