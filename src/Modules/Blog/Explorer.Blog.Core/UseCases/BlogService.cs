using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Blog.API.Public;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogService: IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        public BlogService(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public List<BlogPostDto> GetAll()
        {
            var result = _blogRepository.GetAll();
            return _mapper.Map<List<BlogPostDto>>(result);
        }

        public BlogPostDto GetById(long id)
        {
            var result = _blogRepository.GetById(id);
            return _mapper.Map<BlogPostDto>(result);
        }

        public List<BlogPostDto> GetByAuthor(long authorId)
        {
            var result = _blogRepository.GetByAuthor(authorId);
            return _mapper.Map<List<BlogPostDto>>(result);
        }

        public BlogPostDto Create(CreateBlogPostDto dto, long authorId)
        {
            var post = new BlogPost(
                authorId: authorId, 
                title: dto.Title,
                description: dto.Description,
                createdAt: DateTime.UtcNow
            );

            _blogRepository.Create(post); 

            return _mapper.Map<BlogPostDto>(post);
        }

        public BlogPostDto Update(long id, UpdateBlogPostDto dto, long authorId)
        {
            var post = _blogRepository.GetById(id);
            if (post == null) return null;

            if (post.AuthorId != authorId)
                throw new UnauthorizedAccessException("Unauthorized");

            post.Title = dto.Title;
            post.Description = dto.Description;

            _blogRepository.Update(post);
            return _mapper.Map<BlogPostDto>(post);
        }
        public BlogImageDto AddImage(long postId, BlogImageDto dto)
        {
            var image = _mapper.Map<BlogImage>(dto);
            image.BlogPostId = postId;

            _blogRepository.AddImage(image);
            return _mapper.Map<BlogImageDto>(image);
        }

        public BlogImageDto UpdateImage(BlogImageDto dto)
        {
            var domain = _mapper.Map<BlogImage>(dto);

            var updated = _blogRepository.UpdateImage(domain);
            if (updated == null) return null;

            return _mapper.Map<BlogImageDto>(updated);
        }

        public BlogImageDto GetImage(long id)
        {
            var img = _blogRepository.GetImage(id);
            if (img == null) return null;

            return _mapper.Map<BlogImageDto>(img);
        }
    }
}
