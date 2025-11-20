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
            var post = _blogRepository.GetById(id);
            if (post == null) return null;

            var images = _blogRepository.GetImagesByPostId(id);

            var dto = _mapper.Map<BlogPostDto>(post);
            dto.Images = _mapper.Map<List<BlogImageDto>>(images);

            return dto;
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
            try
            {
                byte[] bytes = null;

                if (!string.IsNullOrWhiteSpace(dto.Base64))
                {
                    var base64 = dto.Base64.Contains(",")
                        ? dto.Base64.Split(',')[1]
                        : dto.Base64;

                    bytes = Convert.FromBase64String(base64);
                }
                else
                {
                    throw new Exception("Base64 data missing");
                }

                var image = new BlogImage(
                    blogPostId: postId,
                    data: bytes,
                    contentType: dto.ContentType,
                    order: dto.Order
                );

                _blogRepository.AddImage(image);

                return _mapper.Map<BlogImageDto>(image);
            }
            catch (Exception ex)
            {
                throw new Exception("AddImage FAILED: " + ex.Message);
            }
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

            return new BlogImageDto
            {
                Id = img.Id,
                Base64 = Convert.ToBase64String(img.Data),
                ContentType = img.ContentType,
                Order = img.Order
            };
        }

        public List<BlogImageDto> GetImagesByPostId(long postId)
        {
            var result = _blogRepository.GetImagesByPostId(postId);

            return result
                .Select(img => new BlogImageDto
                {
                    Id = img.Id,
                    Base64 = Convert.ToBase64String(img.Data),
                    ContentType = img.ContentType,
                    Order = img.Order
                })
                .ToList();
        }

        public bool DeleteImage(long imageId)
        {
            var image = _blogRepository.GetImage(imageId);
            if (image == null) return false;

            _blogRepository.DeleteImage(image);
            return true;
        }

    }
}
