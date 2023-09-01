using Application.Features.Common;

namespace Application.Features.Categories.DTOs
{
    public class CategoryDetailDto : BaseDto, ICategoryDto
    {
        public string Title { get; set; }
        public string MainPhotoUrl { get; set; }
        public List<string> Photos { get; set; }
    }
}
