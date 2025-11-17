using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/tours")]
[ApiController]
public class TourController : ControllerBase
{
    private readonly ITourService _TourService;

    public TourController(ITourService TourService)
    {
        _TourService = TourService;
    }

    [HttpGet]
    public ActionResult<PagedResult<TourDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        return Ok(_TourService.GetPaged(page, pageSize));
    }

    [HttpPost]
    public ActionResult<TourDto> Create([FromBody] TourDto Tour)
    {
        return Ok(_TourService.Create(Tour));
    }

    [HttpPut("{id:long}")]
    public ActionResult<TourDto> Update([FromBody] TourDto Tour)
    {
        return Ok(_TourService.Update(Tour));
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _TourService.Delete(id);
        return Ok();
    }
}