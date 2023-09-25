using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.DTOs
{
    public interface ICategoryDto
    {
        public string Title { get; set; }
        
        public List<GalleryPhotoDto> Photos { get; set; }
    }
}
