using Application.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Projects.DTOs
{
    public class UpdateProjectDto : BaseDto, IProjectDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
        public IFormFile? File { get; set; }
    }
}