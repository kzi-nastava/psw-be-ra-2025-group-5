using Explorer.Blog.API.Dtos.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos.Posts
{
    public class CreateAndPublishBlogPostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<BlogImageDto> Images { get; set; } = new();
    }
}
