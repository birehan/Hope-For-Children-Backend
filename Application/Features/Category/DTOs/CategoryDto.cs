using Application.Features.Common;
using Application.Features.SubCategories.DTOs;

namespace Application.Features.Categories.DTOs
{
    public class CategoryDto : BaseDto, ICategoryDto
    {
        public string Title { get; set; }
        public List<SubCategoryDto> SubCategories { get ; set ; }
    }
}
