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
        List<BlogPostDto> GetAll();
        BlogPostDto GetById(long id);
        List<BlogPostDto> GetByAuthor(long authorId);
        BlogPostDto Create(CreateBlogPostDto dto, long authorId);
        BlogPostDto Update(long id, UpdateBlogPostDto dto, long authorId);
        BlogImageDto AddImage(long postId, BlogImageDto dto);
        BlogImageDto GetImage(long id);
        BlogImageDto UpdateImage(BlogImageDto dto);
    }
}
