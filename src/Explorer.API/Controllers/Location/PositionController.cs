using Explorer.Stakeholders.API.Dtos.Locations;
using Explorer.Stakeholders.API.Public.Positions;
using Explorer.Stakeholders.Core.Domain.Positions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Location;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/positions")]
[ApiController]
public class PositionController : ControllerBase
{
    private readonly IPositionService _PositionService;

    public PositionController(IPositionService PositionService)
    {
        _PositionService = PositionService;
    }

    [HttpGet]
    public ActionResult<Position> GetAll()
    {
        return Ok(_PositionService.GetAll());
    }

    [HttpGet("{touristId:long}")]
    public ActionResult<Position> GetByTourist(long touristId)
    {
        return Ok(_PositionService.GetByTourist(touristId));
    }

    [HttpPost]
    public ActionResult<PositionDto> Create([FromBody] PositionDto Position)
    {
        return Ok(_PositionService.Create(Position));
    }

    [HttpPut("{id:long}")]
    public ActionResult<PositionDto> Update([FromBody] PositionDto Position)
    {
        return Ok(_PositionService.Update(Position));
    }
}