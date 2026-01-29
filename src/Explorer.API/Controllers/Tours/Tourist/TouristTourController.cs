using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/tours")]
[ApiController]
public class TouristTourController : ControllerBase
{
    private readonly ITourService _tourService;

    public TouristTourController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet("{touristId:long}/purchased")]
    public ActionResult<List<TourDto>> GetPurchasedTours(long touristId)
    {
        return Ok(_tourService.GetPurchased(touristId));
    }
}
