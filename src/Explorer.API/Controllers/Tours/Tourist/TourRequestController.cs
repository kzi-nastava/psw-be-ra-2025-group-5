using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Infrastructure.Authentication;

namespace Explorer.API.Controllers.Tours.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tour-request")]
    [ApiController]
    public class TourRequestController : ControllerBase
    {
        private readonly ITourRequestService _tourRequestService;

        public TourRequestController(ITourRequestService tourRequestService)
        {
            _tourRequestService = tourRequestService;
        }

        [HttpPost]
        public ActionResult<TourRequestDto> Create([FromBody] TourRequestDto tourRequest)
        {
            var touristId = User.PersonId();
            tourRequest.TouristId = touristId;

            var result = _tourRequestService.Create(tourRequest);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<TourRequestDto>> GetByTourist([FromQuery] int page, [FromQuery] int pageSize)
        {
            var touristId = User.PersonId();
            var result = _tourRequestService.GetByTourist(touristId, page, pageSize);
            return Ok(result);
        }

        [HttpGet("authors")]
        public ActionResult<List<object>> GetAllAuthors()
        {
            var result = _tourRequestService.GetAllAuthors();
            return Ok(result);
        }
    }
}
