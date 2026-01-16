using Explorer.Payments.API.Dtos.Pricing;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Shopping;

[Route("api/tours/sales")]
[ApiController]
public class TourSaleController : Controller
{
    private readonly ITourSaleService _TourSaleService;

    public TourSaleController(ITourSaleService TourSaleService)
    {
        _TourSaleService = TourSaleService;
    }

    [Authorize(Policy = "authorTouristAdminPolicy")]
    [HttpGet("{saleId:long}")]
    public ActionResult<TourSaleDto> Get(long saleId)
    {
        var result = _TourSaleService.Get(saleId);
        return result is not null ? Ok(result) : NotFound();
    }

    [Authorize(Policy = "authorTouristAdminPolicy")]
    [HttpGet("by-tour/{tourId:long}")]
    public ActionResult<TourSaleDto> GetActiveSaleForTour(long tourId)
    {
        var result = _TourSaleService.GetActiveSaleForTour(tourId);
        return result is not null ? Ok(result) : NotFound();
    }

    [Authorize(Policy = "authorTouristAdminPolicy")]
    [HttpGet("by-author/{authorId:long}")]
    public ActionResult<List<TourSaleDto>> GetByAuthor(long authorId, [FromQuery]bool onlyActive = false)
    {
        return Ok(_TourSaleService.GetByAuthor(authorId, onlyActive));
    }

    [Authorize(Policy = "authorTouristAdminPolicy")]
    [HttpGet("pricing/{tourId:long}")]
    public ActionResult<TourPriceDto> GetFinalPrice(long tourId)
    {
        return Ok(_TourSaleService.GetFinalPrice(tourId));
    }

    [Authorize(Policy = "authorPolicy")]
    [HttpPost]
    public ActionResult<TourSaleDto> Create([FromBody]CreateTourSaleDto sale)
    {
        return Ok(_TourSaleService.Create(sale));
    }

    [Authorize(Policy = "authorPolicy")]
    [HttpPut("{id:long}")]
    public ActionResult<TourSaleDto> Update(long id, [FromBody]TourSaleDto sale)
    {
        return Ok(_TourSaleService.Update(id, sale));
    }

    [Authorize(Policy = "authorPolicy")]
    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _TourSaleService.Delete(id);
        return Ok();
    }
}
