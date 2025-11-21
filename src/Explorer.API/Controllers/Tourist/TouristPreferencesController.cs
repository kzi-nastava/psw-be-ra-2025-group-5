using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/preferences")]
[ApiController]
public class TouristPreferencesController : ControllerBase
{
    private readonly ITouristPreferencesService _service;

    public TouristPreferencesController(ITouristPreferencesService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<TouristPreferencesDto> Get()
    {
        var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
        return Ok(_service.Get(userId));
    }

    [HttpPost]
    public ActionResult<TouristPreferencesDto> Create([FromBody] TouristPreferencesDto dto)
    {
        var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
        dto.UserId = userId;
        return Ok(_service.Create(dto));
    }

    [HttpPut]
    public ActionResult<TouristPreferencesDto> Update([FromBody] TouristPreferencesDto dto)
    {
        var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
        dto.UserId = userId;
        return Ok(_service.Update(dto));
    }

    [HttpDelete]
    public ActionResult Delete()
    {
        var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
        _service.Delete(userId);
        return Ok();
    }
}