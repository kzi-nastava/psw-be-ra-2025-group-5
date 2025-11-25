using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist-locations")]
[ApiController]
public class TouristLocationController : ControllerBase
{
    private readonly ITouristLocationService _TouristLocationService;

    public TouristLocationController(ITouristLocationService TouristLocationService)
    {
        _TouristLocationService = TouristLocationService;
    }

    [HttpGet]
    public ActionResult<TouristLocation> GetAll()
    {
        return Ok(_TouristLocationService.GetAll());
    }

    [HttpGet("{id:long}")]
    public ActionResult<TouristLocation> GetByTourist(long id)
    {
        return Ok(_TouristLocationService.GetByTourist(id));
    }

    [HttpPost]
    public ActionResult<TouristLocationDto> Create([FromBody] TouristLocationDto TouristLocation)
    {
        return Ok(_TouristLocationService.Create(TouristLocation));
    }

    [HttpPut("{id:long}")]
    public ActionResult<TouristLocationDto> Update([FromBody] TouristLocationDto TouristLocation)
    {
        return Ok(_TouristLocationService.Update(TouristLocation));
    }
}