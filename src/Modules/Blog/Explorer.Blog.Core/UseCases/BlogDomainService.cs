using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.Blog.Core.Domain.BlogPosts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Blog.Core.Domain.BlogPosts;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogDomainService
    {
        public void UpdateDraft(BlogPost post, string title, string description)
        {
            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Can only update title/description/images in Draft state.");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.");

            post.Title = title;
            post.Description = description;
        }

        public void AddImage(BlogPost post, BlogImage img)
        {
            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Images can only be added while blog is in Draft state.");

            post.Images.Add(img);
        }

        public void UpdateDescription(BlogPost post, string description)
        {
            if (post.Status != BlogStatus.Published && post.Status != BlogStatus.Active && post.Status != BlogStatus.Famous)
                throw new InvalidOperationException("Only published blogs can update description.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty.");

            post.Description = description;
            post.LastUpdatedAt = DateTime.UtcNow;
        }

        public void Publish(BlogPost post)
        {
            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Only Draft blogs can be published.");

            post.Status = BlogStatus.Published;
        }

        public void Archive(BlogPost post)
        {
            if (post.Status != BlogStatus.Published && post.Status != BlogStatus.Active && post.Status != BlogStatus.Famous)
                throw new InvalidOperationException("Only Published blogs can be archived.");

            post.Status = BlogStatus.Archived;
        }

        public void DeleteImage(BlogPost post, BlogImage img)
        {
            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Images can only be deleted while in Draft state.");

            post.Images.Remove(img);
        }
    }
}
