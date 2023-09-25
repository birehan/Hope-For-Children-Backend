using Application.Features.Common;
using Microsoft.AspNetCore.Http;


namespace Application.Features.Categories.DTOs
{
    public class UpdateCategoryDto : BaseDto, ICategoryDto
    {
        public string Title { get; set; }
        public List<GalleryPhotoDto> Photos { get; set; } = new List<GalleryPhotoDto>();
    }
}
