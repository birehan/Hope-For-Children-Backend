using Application.Features.Common;

namespace Application.Features.Categories.DTOs
{
    public class CategoryDto : BaseDto
    {
        public string Title { get; set; }
        public string MainPhotoUrl { get; set; }
    }
}
