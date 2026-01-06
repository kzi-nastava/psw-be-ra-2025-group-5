using Explorer.Payments.API.Dtos.ShoppingCart;

namespace Explorer.Payments.API.Public;

public interface IShoppingCartService
{
    List<ShoppingCartDto> GetAll();
    ShoppingCartDto GetByTourist(long touristId);
    ShoppingCartDto Create(CreateShoppingCartDto ShoppingCart);
    ShoppingCartDto AddOrderItem(long touristId, long tourId);
    ShoppingCartDto RemoveOrderItem(long touristId, long tourId);
    ShoppingCartDto Checkout(long touristId);
    ShoppingCartDto ApplyCouponToCart(long touristId, string couponCode);
}
