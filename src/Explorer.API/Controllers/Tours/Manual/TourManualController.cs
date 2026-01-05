using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.Core.UseCases.Tours;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.API.Public.Tour;

namespace Explorer.API.Controllers.Tours.Manual;

[Authorize(Policy = "touristPolicy")]
[Route("api/tour-manuals")]
[ApiController]
public class TourManualController : ControllerBase
{
    private readonly ITourManualService _service;

    public TourManualController(ITourManualService service)
    {
        _service = service;
    }

    [HttpGet("{pageKey}")]
    public ActionResult<TourManualStatusDto> GetStatus(string pageKey)
    {
        var userId = GetUserIdFromToken();
        return Ok(_service.GetStatus(userId, pageKey));
    }

    [HttpPost("{pageKey}/seen")]
    public IActionResult MarkAsSeen(string pageKey)
    {
        var userId = GetUserIdFromToken();
        _service.MarkAsSeen(userId, pageKey);
        return Ok();
    }

    private long GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID claim missing in token."); 

        return long.Parse(userIdClaim.Value);
    }

}

