using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tours/{tourId}/reviews")]
    [ApiController]
    public class TourReviewController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourReviewController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourReviewDto>> GetByTour(long tourId, int page = 0, int pageSize = 10)
        {
            var tour = _tourService.Get(tourId);

            var reviews = tour.Reviews
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new PagedResult<TourReviewDto>(reviews, tour.Reviews.Count));
        }

        [HttpPost]
        public ActionResult<TourDto> Create(long tourId, [FromQuery] long userId, [FromQuery] string username, [FromBody] TourReviewDto dto)
        {
            var result = _tourService.AddReview(tourId, userId, username, dto);
            return Ok(result);
        }

        [HttpPut("{reviewId:long}")]
        public ActionResult<TourDto> Update(long tourId, [FromQuery] long userId, long reviewId, [FromBody] TourReviewDto dto)
        {
            var result = _tourService.UpdateReview(tourId, userId, reviewId, dto);
            return Ok(result);
        }

        [HttpDelete("{reviewId:long}")]
        public IActionResult Delete(long tourId, long reviewId)
        {
            _tourService.RemoveReview(tourId, reviewId);
            return NoContent();
        }

        [HttpGet("can-review")]
        public ActionResult<int> CanReview(long tourId, [FromQuery] long userId)
        {
            var canReview = _tourService.GetReviewButtonState(tourId, userId);
            return Ok(canReview);
        }
    }
}
