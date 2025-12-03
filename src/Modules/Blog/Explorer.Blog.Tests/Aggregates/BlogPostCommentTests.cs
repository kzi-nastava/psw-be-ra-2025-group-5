using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

using Explorer.Blog.Core.Domain;        

namespace Explorer.Blog.Tests.Aggregates
{
    public class BlogPostCommentTests
    {
        private BlogPost CreatePublishedBlog()
        {
            var blog = new BlogPost(1, "Test Blog", "Opis test bloga", DateTime.UtcNow);
            blog.Publish(); 
            return blog;
        }

        [Fact]
        public void CanAddCommentToPublishedBlog()
        {
            var blog = CreatePublishedBlog();

            var comment = new Comment(authorId: 1, content: "Super post!");
            blog.AddComment(comment);

            blog.Comments.ShouldContain(comment);
            blog.Comments.Count.ShouldBe(1);
        }

        [Fact]
        public void CannotAddCommentToDraftBlog()
        {
            var blog = new BlogPost(1, "Draft Blog", "Opis draft bloga", DateTime.UtcNow);

            var comment = new Comment(authorId: 1, content: "Super post!");

            Should.Throw<InvalidOperationException>(() => blog.AddComment(comment));
        }

        [Fact]
        public void CanEditCommentWithin15Minutes()
        {
            var blog = CreatePublishedBlog();
            var comment = new Comment(authorId: 1, content: "Stari tekst");
            blog.AddComment(comment);

            comment.UpdateContent("Novi tekst");

            comment.Content.ShouldBe("Novi tekst");
            comment.UpdatedAt.ShouldNotBeNull();
        }

        [Fact]
        public void CannotEditCommentAfter15Minutes()
        {
            var blog = CreatePublishedBlog();
            var comment = new Comment(authorId: 1, content: "Stari tekst");

            typeof(Comment).GetProperty("CreatedAt")!
                .SetValue(comment, DateTimeOffset.UtcNow.AddMinutes(-16));

            blog.AddComment(comment);

            Should.Throw<UnauthorizedAccessException>(() =>
                blog.UpdateComment(comment.CommentId, "Novi tekst", comment.AuthorId));
        }

        [Fact]
        public void CanDeleteCommentWithin15Minutes()
        {
            var blog = CreatePublishedBlog();
            var comment = new Comment(authorId: 1, content: "Za brisanje");
            blog.AddComment(comment);

            blog.RemoveComment(comment.CommentId, comment.AuthorId);
            blog.Comments.ShouldNotContain(comment);
        }

        [Fact]
        public void CannotDeleteCommentAfter15Minutes()
        {
            var blog = CreatePublishedBlog();
            var comment = new Comment(authorId: 1, content: "Za brisanje");

            typeof(Comment).GetProperty("CreatedAt")!
                .SetValue(comment, DateTimeOffset.UtcNow.AddMinutes(-16));

            blog.AddComment(comment);

            Should.Throw<UnauthorizedAccessException>(() =>
                blog.RemoveComment(comment.CommentId, comment.AuthorId));
        }

        [Fact]
        public void CommentStoresAllInformation()
        {
            var comment = new Comment(authorId: 42, content: "Tekst komentara");

            comment.AuthorId.ShouldBe(42);
            comment.Content.ShouldBe("Tekst komentara");
            comment.CreatedAt.ShouldBeLessThanOrEqualTo(DateTime.UtcNow);
            comment.UpdatedAt.ShouldBeNull();
        }
    }
}
