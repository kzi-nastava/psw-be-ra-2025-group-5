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
        List<BlogPostDto> GetAll(long userId);
        BlogPostDto GetById(long id);
        List<BlogPostDto> GetByAuthor(long authorId);
        List<BlogPostDto> GetByStatus(string status);
        BlogPostDto Create(CreateBlogPostDto dto, long authorId);
        BlogPostDto Update(long id, UpdateBlogPostDto dto, long authorId);
        BlogImageDto AddImage(long postId, BlogImageDto dto);
        BlogImageDto GetImage(long id);
        BlogImageDto UpdateImage(BlogImageDto dto);
        List<BlogImageDto> GetImagesByPostId(long postId);
        bool DeleteImage(long imageId);
        BlogPostDto Publish(long id, long authorId);
        BlogPostDto Archive(long id, long authorId);
        BlogPostDto UpdateDraft(long id, UpdateDraftBlogPostDto dto, long authorId);
        BlogPostDto Vote(long blogId, long userId, string voteType);
        BlogPostDto CreateAndPublish(CreateAndPublishBlogPostDto dto, long authorId);

    }
}
