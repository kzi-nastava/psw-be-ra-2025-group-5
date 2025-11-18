using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/touristEquipment")]
[ApiController]
public class TouristEquipmentController : ControllerBase
{
    private readonly ITouristEquipmentService _touristEquipmentService;

    public TouristEquipmentController(ITouristEquipmentService touristEquipmentService)
    {
        _touristEquipmentService = touristEquipmentService;
    }

    [HttpGet]
    public ActionResult<PagedResult<TouristEquipmentDto>> GetOwnedEquipment([FromQuery] long touristId,[FromQuery] int  page, [FromQuery]int pageSize)
    {
        //var touristId = User.PersonId();

        return Ok(_touristEquipmentService.GetPaged(touristId, page, pageSize));
    }

    [HttpPost]
    public ActionResult<TouristEquipmentDto> Create([FromBody] TouristEquipmentDto touristEquipment)
    {
        //touristEquipment.TouristId = User.PersonId();
        return Ok(_touristEquipmentService.Create(touristEquipment));
    }

    [HttpPut("{id:long}")]
    public ActionResult<TouristEquipmentDto> Update([FromBody] TouristEquipmentDto touristEquipment)
    {
        //touristEquipment.TouristId = User.PersonId();
        return Ok(_touristEquipmentService.Update(touristEquipment));
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _touristEquipmentService.Delete(id);
        return Ok();
    }

}
