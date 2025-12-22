using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Shopping;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tour-purchase-token")]
[ApiController]
public class TourPurchaseTokenController : Controller
{
    private readonly ITourPurchaseTokenService _TokenService;

    public TourPurchaseTokenController(ITourPurchaseTokenService TourPurchaseTokenService)
    {
        _TokenService = TourPurchaseTokenService;
    }

    [HttpGet("{touristId:long}")]
    public ActionResult<TourPurchaseToken> GetByTourAndTourist([FromQuery] long tourId, [FromRoute] long touristId)
    {
        var result = _TokenService.GetByTourAndTourist(tourId, touristId);
        return Ok(result); 
    }
}
