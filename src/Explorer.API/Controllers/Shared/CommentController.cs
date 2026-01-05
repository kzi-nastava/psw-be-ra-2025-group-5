using Explorer.Stakeholders.API.Dtos.Comments;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.UseCases;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Explorer.API.Controllers.Shared
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{id:long}")]
        public ActionResult<CommentDto> GetById(long id)
        {
            return Ok(_commentService.GetByCommentId(id));
        }

        [HttpPost]
        public ActionResult<CommentDto> Create([FromBody] CreateCommentDto dto)
        {
            long authorId = GetCurrentUserId();
            var comment = _commentService.Create(authorId, dto);
            return Ok(comment);
        }

        private long GetCurrentUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (claim == null)
                throw new UnauthorizedAccessException("User ID not found.");

            return long.Parse(claim.Value);
        }

    }
}
