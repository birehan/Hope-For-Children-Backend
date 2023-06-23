using Application.Features.SubCategories.DTOs;

namespace Application.Features.Categories.DTOs
{
    public class CreateCategoryDto : ICategoryDto
    {
        public IEnumerable<CreateSubCategoryDto> SubCategories { get; set; }
        public string Title { get ; set ; }
    }
}
