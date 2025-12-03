using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

    [HttpPost("{postId:long}/images")]
    public IActionResult AddImage(long postId, [FromBody] BlogImageDto dto)
    {
        var result = _blogService.AddImage(postId, dto);
        return Ok(result);
    }

    [HttpPut("images/{imageId:long}")]
    public IActionResult UpdateImage(long imageId, [FromBody] BlogImageDto dto)
    {
        dto.Id = imageId;

        var result = _blogService.UpdateImage(dto);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpGet("images/{imageId:long}")]
    public IActionResult GetImage(long imageId)
    {
        var dto = _blogService.GetImage(imageId);
        if (dto == null) return NotFound();

        if (string.IsNullOrWhiteSpace(dto.Base64))
            return BadRequest("Image data missing");

        byte[] data;

        try
        {
            data = Convert.FromBase64String(dto.Base64);
        }
        catch
        {
            return BadRequest("Invalid Base64 format");
        }

        return File(data, dto.ContentType);
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

}

