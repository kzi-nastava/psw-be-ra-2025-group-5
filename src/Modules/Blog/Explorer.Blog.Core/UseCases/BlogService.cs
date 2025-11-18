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

        public BlogPostDto Create(BlogPostDto dto)
        {
            var domain = _mapper.Map<BlogPost>(dto);
            var result = _blogRepository.Create(domain);
            return _mapper.Map<BlogPostDto>(result);
        }

        public BlogPostDto Update(BlogPostDto dto)
        {
            var domain = _mapper.Map<BlogPost>(dto);
            var result = _blogRepository.Update(domain);
            return _mapper.Map<BlogPostDto>(result);
        }
    }
}
