using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public.Shopping;

public interface IShoppingCartService
{
    ShoppingCartDto GetByTourist(long touristId);
    ShoppingCartDto Create(CreateShoppingCartDto ShoppingCart);
}
