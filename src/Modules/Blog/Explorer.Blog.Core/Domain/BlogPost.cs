using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Core.Domain
{
    public class BlogPost : Entity
    {
        public enum BlogStatus
        {
            Draft,      
            Published, 
            Archived   
        }

        public long AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }        
        public List<Comment> Comments { get; set; } = new();  
        public List<BlogImage>? Images { get; set; } = new();
        public BlogStatus Status { get; set; } = BlogStatus.Draft;

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

        // ====================== COMMENT METODE ======================
        public void AddComment(Comment comment)
        {
            if (Status != BlogStatus.Published && Status != BlogStatus.Active && Status != BlogStatus.Famous)
                throw new InvalidOperationException("Cannot add comment to a blog that is not published");

            Comments.Add(comment);

            UpdateStatusByComments();
        }

        public void UpdateStatusByComments()
        {
            if (Comments.Count > 30)
                Status = BlogStatus.Famous;
            else if (Comments.Count > 10)
                Status = BlogStatus.Active;
        }

        public void RemoveComment(long commentId, long requestingUserId)
        {
            var comment = Comments.FirstOrDefault(c => c.CommentId == commentId);
            if (comment == null) throw new Exception("Comment not found");

            if (comment.AuthorId != requestingUserId ||
                (DateTimeOffset.UtcNow - comment.CreatedAt).TotalMinutes > 15)
                throw new UnauthorizedAccessException("Cannot delete this comment");

            Comments.Remove(comment);
        }

        public void UpdateComment(long commentId, string newContent, long requestingUserId)
        {
            var comment = Comments.FirstOrDefault(c => c.CommentId == commentId);
            if (comment == null) throw new Exception("Comment not found");

            if (comment.AuthorId != requestingUserId ||
                (DateTimeOffset.UtcNow - comment.CreatedAt).TotalMinutes > 15)
                throw new UnauthorizedAccessException("Cannot edit this comment");

            comment.UpdateContent(newContent);
        }

        // ====================== PUBLISH ======================
        public void Publish()
        {
            if (Status != BlogStatus.Draft)
                throw new InvalidOperationException("Only draft blogs can be published.");

            Status = BlogStatus.Published;
        }
    }
}
