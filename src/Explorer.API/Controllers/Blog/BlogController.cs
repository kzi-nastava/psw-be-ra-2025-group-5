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
        var posts = _blogService.GetAll();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public IActionResult GetPost(int id)
    {
        var post = _blogService.GetById(id);
        if (post == null) return NotFound();
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
        var result = _blogService.GetImage(imageId);
        return result == null ? NotFound() : Ok(result);
    }
}

