using AutoMapper;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.Blog.Core.Domain.BlogPosts.Entities;
using Explorer.Blog.API.Dtos.Posts;
using Explorer.Blog.API.Dtos.Images;
using Explorer.BuildingBlocks.Core.FileStorage;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogService: IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly BlogDomainService _domainService;
        private readonly IImageStorage _imageStorage;
        public BlogService(IBlogRepository blogRepository, IMapper mapper, BlogDomainService domainService, IImageStorage imageStorage)
        {
            _blogRepository = blogRepository;
            _mapper = mapper; 
            _domainService = domainService;
            _imageStorage = imageStorage;   
        }
        public List<BlogPostDto> GetAll(long userId)
        {
            var result = _blogRepository.GetAll();

            var filtered = result.Where(post =>
                post.Status == BlogStatus.Published ||
                post.Status == BlogStatus.Archived ||
                post.Status == BlogStatus.Active ||
                post.Status == BlogStatus.Famous ||
                post.Status == BlogStatus.ReadOnly ||
                (post.Status == BlogStatus.Draft && post.AuthorId == userId)
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

        public List<BlogPostDto> GetByStatus(string status)
        {
            var result = _blogRepository.GetAll();

            if (Enum.TryParse<BlogStatus>(status, true, out var parsedStatus))
            {
                var filtered = result.Where(post => post.Status == parsedStatus).ToList();
                return _mapper.Map<List<BlogPostDto>>(filtered);
            }

            return new List<BlogPostDto>();
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

                case BlogStatus.Active:
                    throw new InvalidOperationException("Cannot modify active blog");

                case BlogStatus.Famous:
                    throw new InvalidOperationException("Cannot modify famous blog");

                case BlogStatus.ReadOnly:
                    throw new InvalidOperationException("Cannot modify closed blog");

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
            if (post == null)
                throw new KeyNotFoundException("Post not found");

            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Images can be added only to draft blogs");

            var bytes = Convert.FromBase64String(
                dto.Url.Contains(",")
                    ? dto.Url.Split(',')[1]
                    : dto.Url
            );

            var path = _imageStorage.SaveImage("blog", post.AuthorId, bytes, dto.ContentType);

            var image = new BlogImage(
                postId,
                path,
                dto.ContentType,
                dto.Order
            );

            _blogRepository.AddImage(image);
            return _mapper.Map<BlogImageDto>(image);
        }

        public BlogImageDto UpdateImage(long imageId, byte[] imageData, string contentType, int order)
        {
            var image = _blogRepository.GetImage(imageId);
            if (image == null) return null;

            var post = _blogRepository.GetById(image.BlogPostId);
            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Only draft blogs allow image update");

            var newPath = _imageStorage.SaveImage("blog", post.AuthorId, imageData, contentType);

            image.UpdateImage(newPath, contentType);
            image.ChangeOrder(order);

            _blogRepository.UpdateImage(image);

            return _mapper.Map<BlogImageDto>(image);
        }
        public BlogImageDto GetImage(long id)
        {
            var image = _blogRepository.GetImage(id);
            if (image == null) return null;

            return new BlogImageDto
            {
                Id = image.Id,
                ContentType = image.ContentType,
                Order = image.Order,
                Url = image.ImagePath
            };
        }

        public List<BlogImageDto> GetImagesByPostId(long postId)
        {
            return _mapper.Map<List<BlogImageDto>>(
                _blogRepository.GetImagesByPostId(postId)
            );
        }

        public bool DeleteImage(long imageId)
        {
            var image = _blogRepository.GetImage(imageId);
            if (image == null) return false;

            var post = _blogRepository.GetById(image.BlogPostId);
            if (post.Status != BlogStatus.Draft)
                throw new InvalidOperationException("Only draft blogs allow image delete");

            _imageStorage.Delete(image.ImagePath);
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

        public BlogPostDto CreateAndPublish(CreateAndPublishBlogPostDto dto, long authorId)
        {
            var post = new BlogPost(
                authorId,
                dto.Title,
                dto.Description,
                DateTime.UtcNow
            );

            _blogRepository.Create(post);

            _domainService.Publish(post);
            _blogRepository.Update(post);

            return _mapper.Map<BlogPostDto>(post);
        }


    }

}

