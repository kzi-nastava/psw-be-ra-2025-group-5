using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Explorer.Blog.Core.Domain.BlogPost;
using Microsoft.Extensions.Hosting;

namespace Explorer.API.Controllers.Blog;

[Authorize(Policy = "authorOrTouristPolicy")]
[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var userId = GetUserIdFromToken();
        var posts = _blogService.GetAll(userId);
        return Ok(posts);
    }

    [HttpGet("{id:long}")]
    public IActionResult GetPost(long id)
    {
        var post = _blogService.GetById(id);
        if (post == null) return NotFound();

        var userId = GetUserIdFromToken(); 
        if (post.Status == "Draft" && post.AuthorId != userId)
        {
            return Forbid();
        }

        return Ok(post);
    }


    [HttpGet("author/{authorId:long}")]
    public IActionResult GetByAuthor(long authorId)
    {
        var posts = _blogService.GetByAuthor(authorId);
        return Ok(posts);
    }

    [HttpGet("status/{status}")]
    public IActionResult GetByStatus(string status)
    {
        var posts = _blogService.GetByStatus(status);
        return Ok(posts);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateBlogPostDto dto)
    {
        var authorId = GetUserIdFromToken();
        var created = _blogService.Create(dto, authorId);
        return Ok(created);
    }
    private long GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
        return userIdClaim != null ? long.Parse(userIdClaim.Value) : 0;
    }

    [HttpPut("{id:long}")]
    public IActionResult Update(long id, [FromBody] UpdateBlogPostDto dto)
    {
        var authorId = GetUserIdFromToken(); 
        var updated = _blogService.Update(id, dto, authorId);
        return Ok(updated);
    }

    [HttpPost("{postId}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddImage(long postId, [FromForm] BlogImageUploadDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("No file uploaded");

        var post = _blogService.GetById(postId);
        if (post == null) return NotFound("Post not found");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",  "blog", post.AuthorId.ToString());

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.File.FileName);

        var physicalPath = Path.Combine(uploadsFolder, uniqueFileName);

        var relativePath = Path.Combine("images", "blog", uniqueFileName)
                                .Replace("\\", "/");

        using (var fileStream = new FileStream(physicalPath, FileMode.Create))
        {
            await dto.File.CopyToAsync(fileStream);
        }

        var result = _blogService.AddImageFromFile(
            postId,
            relativePath,
            dto.File.ContentType,
            dto.Order
        );

        return Ok(result);
    }

    [HttpPut("{postId}/images/{imageId:long}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateImage(
    long postId,
    long imageId,
    [FromForm] BlogImageUploadUpdateDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("No file uploaded");

        var post = _blogService.GetById(postId);
        if (post == null) return NotFound("Post not found");

        var path = await SaveFile(dto.File);

        var result = _blogService.UpdateImageFromFile(imageId, path, dto.File.ContentType, dto.Order);

        if (result == null) return NotFound();

        return Ok(result);
    }


    [HttpGet("images/{imageId:long}")]
    public IActionResult GetImage(long id)
    {
        var dto = _blogService.GetImage(id);
        if (dto == null) return NotFound();

        return Ok(dto);
    }

    [HttpGet("{postId:long}/images")]
    public IActionResult GetImagesByPostId(long postId)
    {
        var images = _blogService.GetImagesByPostId(postId);
        return Ok(images);
    }

    [HttpDelete("images/{imageId:long}")]
    public IActionResult DeleteImage(long imageId)
    {
        var result = _blogService.DeleteImage(imageId);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPut("{id:long}/publish")]
    public IActionResult Publish(long id)
    {
        var authorId = GetUserIdFromToken();
        var result = _blogService.Publish(id, authorId);
        return Ok(result);
    }

    [HttpPut("{id:long}/archive")]
    public IActionResult Archive(long id)
    {
        var authorId = GetUserIdFromToken();
        var result = _blogService.Archive(id, authorId);
        return Ok(result);
    }

    [HttpPut("{id:long}/draft")]
    public IActionResult UpdateDraft(long id, [FromBody] UpdateDraftBlogPostDto dto)
    {
        var authorId = GetUserIdFromToken();
        var result = _blogService.UpdateDraft(id, dto, authorId);
        return Ok(result);
    }

    [HttpPut("{id:long}/vote/{voteType}")]
    public ActionResult<BlogPostDto> Vote(long id, VoteType voteType)
    {
        var userId = GetUserIdFromToken();
        var result = _blogService.Vote(id, userId, voteType.ToString());

        return Ok(result);
    }

    [HttpPost("publish-with-images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAndPublishWithImages([FromForm] CreateAndPublishBlogPostDto dto)
    {
        var authorId = GetUserIdFromToken();

        // 1️⃣ create draft
        var post = _blogService.Create(
            new CreateBlogPostDto
            {
                Title = dto.Title,
                Description = dto.Description
            },
            authorId
        );

        // 2️⃣ upload images
        if (dto.Images != null && dto.Images.Any())
        {
            int order = 0;
            foreach (var file in dto.Images)
            {
                var path = await SaveFile(file);

                _blogService.AddImageFromFile(
                    post.Id,
                    path,
                    file.ContentType,
                    order++
                );
            }
        }

        var result = _blogService.Publish(post.Id, authorId);
        return Ok(result);
    }
    private async Task<string> SaveFile(IFormFile file)
    {
        var folder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "images",
            "blog");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var physicalPath = Path.Combine(folder, fileName);

        using var stream = new FileStream(physicalPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"images/blog/{fileName}";
    }



}

