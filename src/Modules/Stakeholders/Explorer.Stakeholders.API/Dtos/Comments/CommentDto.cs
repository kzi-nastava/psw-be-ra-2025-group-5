using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos.Comments
{
    public class CommentDto
    {
        public long CommentId { get; set; }
        public long AuthorId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Content { get; set; }
        public string AuthorRole { get; set; } = string.Empty;
    }
}
