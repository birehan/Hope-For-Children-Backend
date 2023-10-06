using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Projects.DTOs
{

    public class UrlsContent
    {
        public string Default { get; set; }

    }

    public class ErrorMessage
    {
        public string message { get; set; }

    }


    public class ContentPhotoDto
    {
        public UrlsContent urls { get; set; }
        public ErrorMessage error { get; set; }

    }
}