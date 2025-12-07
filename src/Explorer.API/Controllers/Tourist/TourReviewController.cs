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
        public ActionResult<TourDto> Create(long tourId, [FromBody] TourReviewDto dto)
        {
            var result = _tourService.AddReview(tourId, dto);
            return Ok(result);
        }

        [HttpPut("{reviewId}")]
        public ActionResult<TourDto> Update(long tourId, long reviewId, [FromBody] TourReviewDto dto)
        {
            var result = _tourService.UpdateReview(tourId, reviewId, dto);
            return Ok(result);
        }

        [HttpDelete("{reviewId}")]
        public IActionResult Delete(long tourId, long reviewId)
        {
            _tourService.RemoveReview(tourId, reviewId);
            return NoContent();
        }
    }
}
