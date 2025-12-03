using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Comment
    {
        public long CommentId { get; private set; }
        public long AuthorId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        public string Content { get; private set; }

        public Comment(long authorId, string content)
        {
            AuthorId = authorId; 
            Content = content;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = null;
            Validate();
        }

        private Comment() { }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                throw new ArgumentException("The comment is empty.", nameof(Content));
            }
        }
    }
}
