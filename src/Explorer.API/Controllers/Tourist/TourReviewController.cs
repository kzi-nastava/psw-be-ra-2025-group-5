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
        private readonly ITourReviewService _tourReviewService;

        public TourReviewController(ITourReviewService service)
        {
            _tourReviewService = service;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourReviewDto>> GetByTour(long tourId, int page = 0, int pageSize = 10)
        {
            var result = _tourReviewService.GetByTour(tourId, page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<TourReviewDto> Create( long tourId, [FromBody] TourReviewDto dto)
        {
            dto.TourID = tourId;
            var result = _tourReviewService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{reviewId}")]
        public ActionResult<TourReviewDto> Update(long tourId, long reviewId, [FromBody] TourReviewDto dto)
        {
            dto.Id = reviewId;
            dto.TourID = tourId;

            var result = _tourReviewService.Update(reviewId, dto);
            return Ok(result);
        }

        [HttpDelete("{reviewId}")]
        public IActionResult Delete(long reviewId)
        {
            _tourReviewService.Delete(reviewId);
            return NoContent();
        }
    }
}
