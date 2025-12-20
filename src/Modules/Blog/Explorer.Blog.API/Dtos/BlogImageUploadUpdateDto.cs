using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class BlogImageUploadUpdateDto
    {
        public IFormFile File { get; set; }
        public int Order { get; set; }
    }
}
