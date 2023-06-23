using Application.Features.Common;
using Application.Photos;
using Domain;

namespace Application.Features.SubCategories.DTOs
{
    public class SubCategoryDto : BaseDto, ISubCategoryDto
    {
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        public string MainPhoto { get; set;}
        public List<string> Photos { get; set; }
    }
}
