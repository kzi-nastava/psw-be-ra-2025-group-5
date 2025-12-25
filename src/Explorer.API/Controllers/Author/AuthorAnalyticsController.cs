using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.API.Internal;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/tours/analytics")]
    [ApiController]
    public class AuthorAnalyticsController : ControllerBase
    {
        private readonly ITourStatisticsService _tourStatisticsService;

        public AuthorAnalyticsController(ITourStatisticsService tourStatisticsService)
        {
            _tourStatisticsService = tourStatisticsService;
        }

        [HttpGet("by-price")]
        public ActionResult<IReadOnlyCollection<ToursByPriceDto>> GetToursCountByPrice([FromQuery] long userId)
        {
            var result = _tourStatisticsService.GetToursCountByPrice(userId);
            return Ok(result);
        }

    }
}
