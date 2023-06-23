using Application.Features.Common;

namespace Application.Features.Alumnis.DTOs
{
    public class AlumniDto : BaseDto, IAlumniDto
    {

        public string Name { get; set; }

        public string Story { get; set; }

        public string PhotoUrl { get; set; }

    }
}