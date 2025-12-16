using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain
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
            if (AuthorId == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(AuthorId));
            }
            if (string.IsNullOrWhiteSpace(Content))
            {
                throw new ArgumentException("The comment is empty.", nameof(Content));
            }
        }

        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Content cannot be empty.");

            Content = newContent;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

    }

}
