using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/search-history")]
[ApiController]
public class SearchHistoryController : ControllerBase
{
    private readonly ITourSearchHistoryService _service;

    public SearchHistoryController(ITourSearchHistoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<TourSearchHistoryDto>> Get()
    {
        var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
        return Ok(_service.GetSearchHistory(userId));
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _service.DeleteSearch(id);
        return Ok();
    }
}

