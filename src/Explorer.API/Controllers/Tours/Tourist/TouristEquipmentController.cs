using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos.Equipments;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/touristEquipment")]
[ApiController]
public class TouristEquipmentController : ControllerBase
{
    private readonly ITouristEquipmentService _touristEquipmentService;
    private readonly ITourService _tourService;

    public TouristEquipmentController(
        ITouristEquipmentService touristEquipmentService,
        ITourService tourService)  
    {
        _touristEquipmentService = touristEquipmentService;
        _tourService = tourService;
    }

    [HttpGet]
    public ActionResult<PagedResult<TouristEquipmentDto>> GetOwnedEquipment([FromQuery] int page, [FromQuery] int pageSize)
    {
        var touristId = User.PersonId();
        return Ok(_touristEquipmentService.GetPaged(touristId, page, pageSize));
    }

    [HttpPost]
    public ActionResult<TouristEquipmentDto> Create([FromBody] TouristEquipmentDto touristEquipment)
    {
        touristEquipment.TouristId = User.PersonId();
        return Ok(_touristEquipmentService.Create(touristEquipment));
    }

    [HttpPut("{id:long}")]
    public ActionResult<TouristEquipmentDto> Update([FromBody] TouristEquipmentDto touristEquipment)
    {
        var touristId = User.PersonId();

        touristEquipment.TouristId = touristId;

        return Ok(_touristEquipmentService.Update(touristEquipment, touristId));
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        var touristId = User.PersonId();

        _touristEquipmentService.Delete(id, touristId);
        return Ok();
    }

    [HttpGet("tour/{tourId:long}/required-equipment")]
    public ActionResult<List<RequiredEquipmentDto>> GetRequiredEquipmentForTour(long tourId)
    {
        var requiredEquipment = _tourService.GetRequiredEquipment(tourId);
        return Ok(requiredEquipment);
    }

}