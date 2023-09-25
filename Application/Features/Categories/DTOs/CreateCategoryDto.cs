using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.DTOs
{
    public class CreateCategoryDto : ICategoryDto
    {
        public string Title { get; set; }
        public List<GalleryPhotoDto> Photos { get; set; } = new List<GalleryPhotoDto>();
    }
}
