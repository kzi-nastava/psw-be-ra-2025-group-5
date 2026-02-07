using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/suggestions")]
[ApiController]
public class TourSuggestionsController : ControllerBase
{
    private readonly ITourSuggestionService _service;

    public TourSuggestionsController(ITourSuggestionService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<PagedResult<TourDto>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
        return Ok(_service.GetSuggestedTours(userId, page, pageSize));
    }
}
