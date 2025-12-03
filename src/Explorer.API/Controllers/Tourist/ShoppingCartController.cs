using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Shopping;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/shopping-cart")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService _ShoppingCartService;

    public ShoppingCartController(IShoppingCartService ShoppingCartService)
    {
        _ShoppingCartService = ShoppingCartService;
    }

    [HttpGet("{touristId:long}")]
    public ActionResult<ShoppingCart> GetByTourist(long touristId)
    {
        return Ok(_ShoppingCartService.GetByTourist(touristId));
    }

    [HttpPost]
    public ActionResult<ShoppingCartDto> Create([FromBody] CreateShoppingCartDto ShoppingCart)
    {
        return Ok(_ShoppingCartService.Create(ShoppingCart));
    }

    [HttpPost("{touristId:long}/items/{tourId:long}")]
    public ActionResult<TourDto> AddOrderItem(long touristId, long tourId)
    {
        var result = _ShoppingCartService.AddOrderItem(touristId, tourId);
        return Ok(result);
    }

    [HttpDelete("{touristId:long}/items/{tourId:long}")]
    public ActionResult<TourDto> RemoveOrderItem(long touristId, long tourId)
    {
        var result = _ShoppingCartService.RemoveOrderItem(touristId, tourId);
        return Ok(result);
    }
}