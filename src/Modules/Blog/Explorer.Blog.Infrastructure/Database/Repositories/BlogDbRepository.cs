using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Blog.Infrastructure.Database.Repositories
{
    public class BlogDbRepository : IBlogRepository
    {
        private readonly BlogContext _dbContext;

        public BlogDbRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BlogPost> GetAll() => _dbContext.BlogPosts.Include(x => x.Images).ToList();
        public BlogPost? GetById(long id) => _dbContext.BlogPosts
        .Include(b => b.Images)
        .Include(b => b.Comments)
        .FirstOrDefault(blog => blog.Id == id);


        public List<BlogPost> GetByAuthor(long authorId) => _dbContext.BlogPosts.Where(author => author.AuthorId == authorId).ToList();
        public BlogPost Create(BlogPost blog)
        {
            _dbContext.BlogPosts.Add(blog);
            _dbContext.SaveChanges();
            return blog;
        }
        public BlogPost Update(BlogPost blog)
        {
            _dbContext.BlogPosts.Update(blog);
            _dbContext.SaveChanges();
            return blog;
        }
        public void AddImage(BlogImage image)
        {
            _dbContext.BlogImages.Add(image);
            _dbContext.SaveChanges();
        }
        public BlogImage UpdateImage(BlogImage image)
        {
            var existing = _dbContext.BlogImages.FirstOrDefault(x => x.Id == image.Id);
            if (existing == null) return null;

            existing.ContentType = image.ContentType;
            existing.Order = image.Order;
            existing.Data = image.Data;

            _dbContext.SaveChanges();
            return existing;
        }

        public BlogImage? GetImage(long id) => _dbContext.BlogImages.FirstOrDefault(x => x.Id == id);

        public List<BlogImage> GetImagesByPostId(long postId)
        {
            return _dbContext.BlogImages
                .Where(i => i.BlogPostId == postId)
                .OrderBy(i => i.Order)
                .ToList();
        }

        public void DeleteImage(BlogImage image)
        {
            _dbContext.BlogImages.Remove(image);
            _dbContext.SaveChanges();
        }

        public List<Comment> GetComments(long blogId)
        {
            var blog = _dbContext.BlogPosts.Include(b => b.Comments).FirstOrDefault(b => b.Id == blogId);
            return blog?.Comments ?? new List<Comment>();
        }

        public void AddComment(long blogId, Comment comment)
        {
            var blog = _dbContext.BlogPosts
                .Include(b => b.Comments)
                .FirstOrDefault(b => b.Id == blogId);

            if (blog == null) throw new Exception("Blog not found");

            blog.Comments.Add(comment);

            var entry = _dbContext.Entry(comment);
            if (entry != null)
            {
                entry.Property("BlogPostId").CurrentValue = blogId;
            }

            _dbContext.SaveChanges();
        }



        public void RemoveComment(long blogId, long commentId)
        {
            var blog = _dbContext.BlogPosts.Include(b => b.Comments).FirstOrDefault(b => b.Id == blogId);
            if (blog == null) throw new Exception("Blog not found");

            var comment = blog.Comments.FirstOrDefault(c => c.CommentId == commentId);
            if (comment != null)
            {
                blog.Comments.Remove(comment);
                _dbContext.SaveChanges();
            }
        }




    }
}
