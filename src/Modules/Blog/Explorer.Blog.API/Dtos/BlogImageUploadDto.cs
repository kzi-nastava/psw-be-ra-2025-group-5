using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;



namespace Explorer.Blog.API.Dtos
{
    public class BlogImageUploadDto
    {
        public IFormFile File { get; set; }
        public int Order { get; set; }
    }
}
