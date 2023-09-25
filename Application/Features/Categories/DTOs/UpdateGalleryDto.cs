using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Common;

using Microsoft.AspNetCore.Http;



namespace Application.Features.Categories.DTOs
{
    public class UpdateGalleryDto : BaseDto
    {
        public string Title { get; set; }
        public int MainPhotoIndex { get; set; } = -1;
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();

        public int OldMainPhotoIndex { get; set; } = -1;
        public List<string> OldPhotos { get; set; } = new List<string>();
    }
}