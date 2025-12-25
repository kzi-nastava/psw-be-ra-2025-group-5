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

        //[HttpGet("purchased/count/{userId:long}")]
        //public ActionResult<int> GetPurchasedToursCount(long userId)
        //{
        //    return Ok(_tourStatisticsService.GetPurchasedToursCount(userId));
        //}

        //[HttpGet("completed/{userId:long}")]
        //public ActionResult<IReadOnlyCollection<TourStatisticsItemDto>> GetCompletedTours(long userId)
        //{
        //    return Ok(_tourStatisticsService.GetCompletedTours(userId));
        //}
    }
}
