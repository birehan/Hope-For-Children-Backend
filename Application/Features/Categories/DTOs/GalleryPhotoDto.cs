using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.DTOs
{
    public class GalleryPhotoDto
    {
        public bool IsMainPhoto { get; set; }
        public IFormFile? File { get; set; }
        public string? PhotoUrl { get; set; }
    }
}