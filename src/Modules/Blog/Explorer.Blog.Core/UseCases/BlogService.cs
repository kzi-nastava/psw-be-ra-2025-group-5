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
using static Explorer.Blog.Core.Domain.BlogPost;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogService: IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly BlogDomainService _domainService;
        public BlogService(IBlogRepository blogRepository, IMapper mapper, BlogDomainService domainService)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _domainService = domainService;
        }
        public List<BlogPostDto> GetAll(long userId)
        {
            var result = _blogRepository.GetAll();

            var filtered = result.Where(post =>
                post.Status == BlogPost.BlogStatus.Published ||
                post.Status == BlogPost.BlogStatus.Archived ||
                (post.Status == BlogPost.BlogStatus.Draft && post.AuthorId == userId)
            ).ToList();

            return _mapper.Map<List<BlogPostDto>>(filtered);
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

            switch (post.Status)
            {
                case BlogStatus.Published:
                    _domainService.UpdateDescription(post, dto.Description);
                    break;

                case BlogStatus.Draft:
                    throw new InvalidOperationException("Draft posts must be updated through UpdateDraft");

                case BlogStatus.Archived:
                    throw new InvalidOperationException("Cannot modify archived blog");

                default:
                    throw new InvalidOperationException("Unknown blog state");
            }

            _blogRepository.Update(post);
            return _mapper.Map<BlogPostDto>(post);
        }

        public BlogPostDto UpdateDraft(long id, UpdateDraftBlogPostDto dto, long authorId)
        {
            var post = _blogRepository.GetById(id);
            if (post == null) return null;

            if (post.AuthorId != authorId)
                throw new UnauthorizedAccessException("Unauthorized");

            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Only draft posts can use UpdateDraft");

            _domainService.UpdateDraft(post, dto.Title, dto.Description);

            _blogRepository.Update(post);
            return _mapper.Map<BlogPostDto>(post);
        }

        public BlogImageDto AddImage(long postId, BlogImageDto dto)
        {
            var post = _blogRepository.GetById(postId);
            if (post == null) throw new Exception("Post not found");

            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Cannot add images unless post is in Draft");
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
            var image = _blogRepository.GetImage(dto.Id);
            if (image == null) return null;

            var post = _blogRepository.GetById(image.BlogPostId);

            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Cannot update images unless post is in Draft");

            var domain = _mapper.Map<BlogImage>(dto);
            var updated = _blogRepository.UpdateImage(domain);

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

            var post = _blogRepository.GetById(image.BlogPostId);

            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Cannot delete images unless post is in Draft");

            _blogRepository.DeleteImage(image);
            return true;
        }

        public BlogPostDto Publish(long id, long authorId)
        {
            var post = _blogRepository.GetById(id);
            if (post == null) return null;

            if (post.AuthorId != authorId)
                throw new UnauthorizedAccessException("Unauthorized");

            _domainService.Publish(post);
            _blogRepository.Update(post);

            return _mapper.Map<BlogPostDto>(post);
        }
        
        public BlogPostDto Archive(long id, long authorId)
        {
            var post = _blogRepository.GetById(id);
            if (post == null) return null;

            if (post.AuthorId != authorId)
                throw new UnauthorizedAccessException("Unauthorized");

            _domainService.Archive(post);
            _blogRepository.Update(post);

            return _mapper.Map<BlogPostDto>(post);
        }

        public BlogPostDto Vote(long blogId, long userId, string voteTypeStr)
        {
            var voteType = Enum.Parse<VoteType>(voteTypeStr, true);

            var blog = _blogRepository.GetById(blogId) ?? throw new KeyNotFoundException("Blog not found");

            blog.Vote(userId, voteType);
            _blogRepository.Update(blog);

            return _mapper.Map<BlogPostDto>(blog);
        }

    }
}
