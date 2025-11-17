using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain
{
    public class BlogImage: Entity 
    {
        public long BlogPostId { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int Order { get; set; }

        public BlogImage(long blogPostId, byte[] data, string contentType, string fileName, int order)
        {
            BlogPostId = blogPostId;
            Data = data;
            ContentType = contentType;
            FileName = fileName;
            Order = order;
            Validate(); 
        }

        private void Validate()
        {
            if (BlogPostId == 0) throw new ArgumentException("Invalid BlogPostId");
            if (Data is null) throw new ArgumentNullException("Invalid data");
            if (string.IsNullOrEmpty(FileName)) throw new ArgumentException("Invalid filename");
            if (!string.IsNullOrEmpty(ContentType)) throw new ArgumentException("Invalid content type");
        }
    }
}
