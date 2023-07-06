using Microsoft.AspNetCore.Http;

namespace Application.Features.Projects.DTOs
{
    public class CreateProjectDto : IProjectDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? PdfFile { get; set; }
        public IFormFile ImageFile { get; set; }

    }
}