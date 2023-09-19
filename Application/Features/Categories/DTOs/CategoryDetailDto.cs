using Application.Features.Common;
using Domain;

namespace Application.Features.Categories.DTOs
{
    public class CategoryDetailDto : BaseDto
    {
        public string Title { get; set; }
        public string MainPhotoUrl { get; set; }
        public List<GalleryPhoto> Photos { get; set; }
    }
}
