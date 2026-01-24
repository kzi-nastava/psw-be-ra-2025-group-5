using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.Users;
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
        private readonly ITourExecutionService _tourExecutionService;

        public PublicTourController(ITourService tourService, ITourExecutionService tourExecutionService)
        {
            _tourService = tourService;
            _tourExecutionService = tourExecutionService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDto>> GetAllPublished([FromQuery] int page, [FromQuery] int pageSize)
        {
            var allPublishedTours = _tourService.GetPagedPublished(page, pageSize);
            List<TourDto> availableTours;


            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // korisnik je ulogovan → filtriraj kupljene ture
                long userId = long.Parse(User.FindFirst("id")!.Value);


                var purchasedTours = _tourExecutionService.GetPurchasedToursWithoutExecution(userId);


                availableTours = allPublishedTours.Results
                .Where(t => !purchasedTours.Any(pt => pt.Id == t.Id))
                .ToList();
            }
            else
            {
                // anonimni korisnik → sve ture su dostupne
                availableTours = allPublishedTours.Results;
            }

            return Ok(new PagedResult<TourDto>(availableTours, availableTours.Count));
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
    }

}
