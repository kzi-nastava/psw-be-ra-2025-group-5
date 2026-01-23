using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Infrastructure.Authentication;

namespace Explorer.API.Controllers.Tours.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tour-request")]
    [ApiController]
    public class TourRequestController : ControllerBase
    {
        private readonly ITourRequestService _tourRequestService;

        public TourRequestController(ITourRequestService tourRequestService)
        {
            _tourRequestService = tourRequestService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourRequestDto>> GetForAuthor(
            [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            var authorId = User.PersonId();
            var result = _tourRequestService.GetByAuthor(authorId, page, pageSize);
            return Ok(result);
        }

        [HttpPost("{id:long}/accept")]
        public ActionResult<TourRequestDto> Accept(long id)
        {
            var authorId = User.PersonId();
            var result = _tourRequestService.Accept(id, authorId);
            return Ok(result);
        }

        [HttpPost("{id:long}/decline")]
        public ActionResult<TourRequestDto> Decline(long id)
        {
            var authorId = User.PersonId();
            var result = _tourRequestService.Decline(id, authorId);
            return Ok(result);
        }
    }
}
