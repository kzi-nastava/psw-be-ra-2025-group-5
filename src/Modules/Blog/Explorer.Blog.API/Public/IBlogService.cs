using Explorer.Blog.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Public
{
    public interface IBlogService
    {
        BlogPostDto GetById(long id);
        List<BlogPostDto> GetByAuthor(long authorId);
        BlogPostDto Create(BlogPostDto blog);
        BlogPostDto Update(BlogPostDto blog);
    }
}
