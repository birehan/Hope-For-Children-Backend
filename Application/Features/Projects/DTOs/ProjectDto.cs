using Application.Features.Common;

namespace Application.Features.Projects.DTOs
{
    public class ProjectDto : BaseDto, IProjectDto
    {

        public string Title { get; set; }


        public string Description { get; set; }


        public string PhotoUrl { get; set; }

        public string Content { get; set; }

    }
}