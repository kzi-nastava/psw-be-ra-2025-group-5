using Explorer.Tours.API.Dtos.Shoppings;

namespace Explorer.Tours.API.Public.Shopping;

public interface IShoppingCartService
{
    List<ShoppingCartDto> GetAll();
    ShoppingCartDto GetByTourist(long touristId);
    ShoppingCartDto Create(CreateShoppingCartDto ShoppingCart);
    ShoppingCartDto AddOrderItem(long touristId, long tourId);
    ShoppingCartDto RemoveOrderItem(long touristId, long tourId);
    ShoppingCartDto Checkout(long touristId);
}
