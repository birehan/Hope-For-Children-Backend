using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application.Features.Categories.DTOs
{
public class GalleryPhotoDto
{
    public bool IsMainPhoto { get; set; }

    // [BindProperty(Name = "Photos[0][File]")]
    public IFormFile? File { get; set; }

    public string? PhotoUrl { get; set; }
}
}