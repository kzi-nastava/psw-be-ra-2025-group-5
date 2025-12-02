using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class CommentBlogDto
    {
        public long CommentId { get; set; }       
        public long AuthorId { get; set; }        
        public string Content { get; set; }        
        public DateTimeOffset CreatedAt { get; set; }   
        public DateTimeOffset? UpdatedAt { get; set; }  
    }
}
