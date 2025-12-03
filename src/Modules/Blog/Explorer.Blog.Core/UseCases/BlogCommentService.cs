using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogCommentService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogCommentService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public List<Comment> GetAll(long blogId) => _blogRepository.GetComments(blogId);

        public void AddComment(long blogId, long authorId, string content)
        {
            var blog = _blogRepository.GetById(blogId);
            if (blog.Status != BlogStatus.Published && blog.Status != BlogStatus.Active && blog.Status != BlogStatus.Famous)
                throw new InvalidOperationException("Cannot add comment to a blog that is not published");

            var comment = new Comment(authorId, content);
            blog.AddComment(comment); 
            _blogRepository.Update(blog);
        }
        public void UpdateComment(long blogId, long commentId, long authorId, string newContent)
        {
            var blog = _blogRepository.GetById(blogId);
            blog.UpdateComment(commentId, newContent, authorId);
            _blogRepository.Update(blog);
        }

        public void DeleteComment(long blogId, long commentId, long authorId)
        {
            var blog = _blogRepository.GetById(blogId);
            blog.RemoveComment(commentId, authorId);
            _blogRepository.Update(blog);
        }
    }

}
