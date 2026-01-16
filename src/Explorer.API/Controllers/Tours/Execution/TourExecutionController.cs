using Explorer.Stakeholders.API.Dtos.Tours.Problems;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Locations;
using Explorer.Tours.API.Dtos.Tours.Executions;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Execution;

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

    [HttpPost("start/{tourId:long}")]
    public ActionResult<StartExecutionResultDto> Start(long tourId)
    {
        long userId = long.Parse(User.FindFirst("id")!.Value);

        var result = _service.StartExecution(userId, tourId);
        return Ok(result);
    }

    [HttpPost("check-proximity/{executionId:long}")]
    public ActionResult<CheckProximityDto> CheckProximity(
        long executionId,
        [FromBody] LocationDto location)
    {
        var result = _service.CheckProximity(executionId, location);
        return Ok(result);
    }

    [HttpPost("complete/{executionId:long}")]
    public IActionResult Complete(long executionId)
    {
        _service.CompleteExecution(executionId);
        return Ok();
    }

    [HttpPost("abandon/{executionId:long}")]
    public IActionResult Abandon(long executionId)
    {
        _service.AbandonExecution(executionId);
        return Ok();
    }

    [HttpGet("{executionId:long}")]
    public ActionResult<TourExecutionDto> Get(long executionId)
    {
        var dto = _service.GetExecution(executionId);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpGet]
    public ActionResult<List<TourExecutionDto>> GetForUser()
    {
        long userId = long.Parse(User.FindFirst("id")!.Value);
        var executions = _service.GetExecutionsForUser(userId);
        return Ok(executions);
    }

    [HttpGet("purchased")]
    public ActionResult<List<TourBasicDto>> GetPurchasedTours()
    {
        long userId = long.Parse(User.FindFirst("id")!.Value);
        var tours = _service.GetPurchasedToursWithoutExecution(userId);
        return Ok(tours);
    }


}
