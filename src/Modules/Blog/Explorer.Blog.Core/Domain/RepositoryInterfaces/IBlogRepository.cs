using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain.RepositoryInterfaces
{
    public interface IBlogRepository
    {
        BlogPost? GetById(long id);
        List<BlogPost> GetByAuthor(long authorId);
        BlogPost Create(BlogPost blog);
        BlogPost Update(BlogPost blog);
    }
}
