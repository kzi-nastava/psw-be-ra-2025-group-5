using Explorer.Payments.API.Dtos.ShoppingCart;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Shopping;

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
    public ActionResult<ShoppingCartDto> GetByTourist(long touristId)
    {
        var result = _ShoppingCartService.GetByTourist(touristId);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public ActionResult<ShoppingCartDto> Create([FromBody] CreateShoppingCartDto ShoppingCart)
    {
        return Ok(_ShoppingCartService.Create(ShoppingCart));
    }

    [HttpPost("{touristId:long}/items/{tourId:long}")]
    public ActionResult<ShoppingCartDto> AddOrderItem(long touristId, long tourId)
    {
        try
        {
            var result = _ShoppingCartService.AddOrderItem(touristId, tourId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("{touristId:long}/gift-items")]
    public ActionResult<ShoppingCartDto> AddGiftItem(
        long touristId,
        [FromBody] AddGiftItemDto request)
    {
        try
        {
            var result = _ShoppingCartService.AddGiftItem(
                touristId,
                request.RecipientId,
                request.TourId,
                request.GiftMessage);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{touristId:long}/items/{tourId:long}")]
    public ActionResult<ShoppingCartDto> RemoveOrderItem(
        long touristId,
        long tourId,
        [FromQuery] long? recipientId = null)
    {
        try
        {
            var result = _ShoppingCartService.RemoveOrderItem(touristId, tourId, recipientId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{touristId:long}/checkout")]
    public ActionResult<ShoppingCartDto> Checkout(long touristId)
    {
        try
        {
            var result = _ShoppingCartService.Checkout(touristId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{touristId:long}/apply-coupon")]
    public ActionResult<ShoppingCartDto> ApplyCoupon(long touristId, [FromQuery] string couponCode)
    {
        try
        {
            var result = _ShoppingCartService.ApplyCouponToCart(touristId, couponCode);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}