using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours
{
    [AllowAnonymous]
    [Route("api/public/tours")]
    [ApiController]
    public class PublicTourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public PublicTourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDto>> GetAllPublished([FromQuery] int page, [FromQuery] int pageSize)
        {
            return Ok(_tourService.GetPagedPublished(page, pageSize));
        }

        [HttpGet("batch")]
        public ActionResult<List<TourDto>> GetMultiple([FromQuery] long[] ids)
        {
            return Ok(_tourService.GetMultiple(ids));
        }

        [HttpGet("{id:long}")]
        public ActionResult<TourDto> GetPublished(long id)
        {
            return Ok(_tourService.GetPublished(id));
        }

        [HttpGet("{tourId:long}/keypoints/{keyPointId:long}")]
        public ActionResult<KeyPointDto> GetKeyPoint(long tourId, long keyPointId)
        {
            var result = _tourService.GetKeyPoint(tourId, keyPointId);
            return Ok(result);
        }

        [Authorize] 
        [HttpPost("premium-wheel")]
        public ActionResult<TourDto> SpinWheel()
        {
            try
            {
                var userId = long.Parse(User.FindFirst("id")!.Value);
                var result = _tourService.SpinPremiumWheel(userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }


    }

}
