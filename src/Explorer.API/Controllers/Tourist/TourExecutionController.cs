using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/execution")]
[ApiController]
public class TourExecutionController : ControllerBase
{
    private readonly ITourExecutionService _service;

    public TourExecutionController(ITourExecutionService service)
    {
        _service = service;
    }

    // POST api/tour-execution/start/{tourId}
    [HttpPost("start/{tourId:long}")]
    public ActionResult<StartExecutionResultDto> Start(long tourId)
    {
        long userId = long.Parse(User.FindFirst("id")!.Value);

        var result = _service.StartExecution(userId, tourId);
        return Ok(result);
    }

    // POST api/tour-execution/check-proximity/{executionId}
    [HttpPost("check-proximity/{executionId:long}")]
    public ActionResult<CheckProximityDto> CheckProximity(
        long executionId,
        [FromBody] LocationDto location)
    {
        var result = _service.CheckProximity(executionId, location);
        return Ok(result);
    }

    // POST api/tour-execution/complete/{executionId}
    [HttpPost("complete/{executionId:long}")]
    public IActionResult Complete(long executionId)
    {
        _service.CompleteExecution(executionId);
        return Ok();
    }

    // POST api/tour-execution/abandon/{executionId}
    [HttpPost("abandon/{executionId:long}")]
    public IActionResult Abandon(long executionId)
    {
        _service.AbandonExecution(executionId);
        return Ok();
    }

    // GET api/tour-execution/{executionId}
    [HttpGet("{executionId:long}")]
    public ActionResult<TourExecutionDto> Get(long executionId)
    {
        var dto = _service.GetExecution(executionId);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

}
