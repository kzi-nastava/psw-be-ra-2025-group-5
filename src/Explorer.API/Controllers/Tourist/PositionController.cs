using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

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