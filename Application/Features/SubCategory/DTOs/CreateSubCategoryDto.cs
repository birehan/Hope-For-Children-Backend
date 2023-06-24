using Microsoft.AspNetCore.Http;

namespace Application.Features.SubCategories.DTOs
{
    public class CreateSubCategoryDto : ISubCategoryDto
    {
        public string Title { get; set; }
        public string CategoryTitle { get; set; }
        public IFormFile MainPhoto { get; set; }
        public List<IFormFile> Photos { get ; set ; }
    }
}
