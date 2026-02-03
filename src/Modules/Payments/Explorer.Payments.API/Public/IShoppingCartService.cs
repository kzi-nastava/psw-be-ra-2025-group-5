using Explorer.Payments.API.Dtos.ShoppingCart;

namespace Explorer.Payments.API.Public;

public interface IShoppingCartService
{
    List<ShoppingCartDto> GetAll();
    ShoppingCartDto GetByTourist(long touristId);
    ShoppingCartDto Create(CreateShoppingCartDto entity);
    ShoppingCartDto AddOrderItem(long touristId, long tourId);
    ShoppingCartDto AddGiftItem(long touristId, long recipientId, long tourId, string? giftMessage = null); 
    ShoppingCartDto RemoveOrderItem(long touristId, long tourId, long? recipientId = null);
    ShoppingCartDto Checkout(long touristId);
    ShoppingCartDto ApplyCouponToCart(long touristId, string couponCode);
    ShoppingCartDto CheckoutAsGift(long donorId, long recipientId, long tourId);
}
