using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain
{
    public class BlogPost: Entity 
    {
        public long AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<BlogImage>? Images { get; set; }

        public BlogPost(long authorId, string title, string description, DateTime createdAt) 
        { 
            AuthorId = authorId;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Validate(); 
        }

        private void Validate()
        {
            if (AuthorId == 0) throw new ArgumentException("Invalid AuthorId");
            if (string.IsNullOrWhiteSpace(Title)) throw new ArgumentException("Missing title");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Missing description");
        }

    }
}