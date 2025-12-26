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
    }

}
