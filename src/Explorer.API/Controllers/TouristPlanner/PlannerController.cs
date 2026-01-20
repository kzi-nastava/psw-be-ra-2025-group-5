using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.TouristPlanner;

[Authorize(Policy = "touristPolicy")]
[Route("api/planner")]
[ApiController]
public class PlannerController : ControllerBase
{
    private readonly IPlannerService _plannerService;

    public PlannerController(IPlannerService plannerService)
    {
        _plannerService = plannerService;
    }

    [HttpGet("{touristId:long}")]
    public ActionResult<PlannerDto> GetPlanner(long touristId)
    {
        return Ok(_plannerService.GetOrCreatePlanner(touristId));
    }

    [HttpGet("{touristId:long}/{date}")]
    public ActionResult<PlannerDayDto> GetDay(long touristId, DateOnly date)
    {
        try
        {
            return Ok(_plannerService.GetDay(touristId, date));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{touristId:long}/{date:datetime}/timeblocks")]
    public ActionResult<PlannerDayDto> AddBlock(long touristId, DateOnly date, [FromBody] CreatePlannerTimeBlockDto dto)
    {
        try
        {
            return Ok(_plannerService.AddBlock(touristId, date, dto));
        }
        catch (Exception ex)
        { 
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{touristId:long}/{date:datetime}/timeblocks/{blockId:long}")]
    public ActionResult<PlannerDayDto> RescheduleBlock(long touristId, DateOnly date, long blockId, [FromBody] CreatePlannerTimeBlockDto dto)
    {
        try
        {
            return Ok(_plannerService.RescheduleBlock(touristId, date, blockId, dto));
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{touristId:long}/{date:datetime}/timeblocks/{blockId:long}")]
    public ActionResult RemoveBlock(long touristId, DateOnly date, long blockId)
    {
        try
        {
            _plannerService.RemoveBlock(touristId, date, blockId);
            return Ok();
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
        }
    }
}
