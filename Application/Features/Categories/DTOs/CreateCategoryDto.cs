using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.DTOs
{
    public class CreateCategoryDto : ICategoryDto
    {
        public string Title { get; set; }
        public IFormFile MainPhoto { get; set; }
        public List<IFormFile>? Photos { get; set; }
    }
}
