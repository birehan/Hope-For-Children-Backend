using Application.Features.Common;
using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.Features.SubCategories.DTOs
{
    public class UpdateSubCategoryDto : BaseDto, ISubCategoryDto
    {
        public string? Title { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public List<IFormFile>? Photos { get ; set ; }
    }
}
