using Microsoft.AspNetCore.Http;

namespace Explorer.Blog.API.Dtos.Posts
{
    public class CreateAndPublishBlogPostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; }

    }
}
