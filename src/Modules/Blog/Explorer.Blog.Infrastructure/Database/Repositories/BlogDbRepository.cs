using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;

namespace Explorer.Blog.Infrastructure.Database.Repositories
{
    public class BlogDbRepository: IBlogRepository
    {
        private readonly BlogContext _dbContext;

        public BlogDbRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BlogPost? GetById(long id) => _dbContext.BlogPosts.FirstOrDefault(blog => blog.Id == id);
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
    }
}
