using Application.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Alumnis.DTOs
{
    public class UpdateAlumniDto : BaseDto, IAlumniDto
    {
        public string Name { get; set; }
        public string Story { get; set; }
        public IFormFile? File { get; set; }
    }
}