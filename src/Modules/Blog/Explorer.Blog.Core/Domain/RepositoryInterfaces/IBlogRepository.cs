using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain.RepositoryInterfaces
{
    public interface IBlogRepository
    {
        List<BlogPost> GetAll();
        BlogPost? GetById(long id);
        List<BlogPost> GetByAuthor(long authorId);
        BlogPost Create(BlogPost blog);
        BlogPost Update(BlogPost blog);
        void AddImage(BlogImage image);
        BlogImage? GetImage(long id);
        BlogImage UpdateImage(BlogImage image);
        List<BlogImage> GetImagesByPostId(long postId);
        void DeleteImage(BlogImage image); 
    }
}
