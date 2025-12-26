using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Blog.Core.Domain.BlogPosts.Entities
{
    public class BlogImage: Entity 
    {
        public long BlogPostId { get; set; }
        public string ImagePath { get; set; }
        public string ContentType { get; set; }
        public int Order { get; set; }
        public BlogImage() { }
        public BlogImage(long blogPostId, string imagePath, string contentType, int order)
        {
            BlogPostId = blogPostId;
            ImagePath = imagePath;
            ContentType = contentType;
            Order = order;
            Validate(); 
        }
        public void ChangeOrder(int order)
        {
            Order = order;
        }
        public void UpdateImagePath(string newPath)
        {
            if (string.IsNullOrWhiteSpace(newPath))
                throw new ArgumentException("Invalid path");
            ImagePath = newPath;
        }
        public void UpdateImage(string filePath, string contentType)
        {
            ImagePath = filePath;
            ContentType = contentType;
        }

        private void Validate()
        {
            if (BlogPostId == 0) throw new ArgumentException("Invalid BlogPostId");
            if (string.IsNullOrWhiteSpace(ImagePath)) throw new ArgumentException("Invalid FilePath");
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentException("Invalid content type");
        }
    }
}
