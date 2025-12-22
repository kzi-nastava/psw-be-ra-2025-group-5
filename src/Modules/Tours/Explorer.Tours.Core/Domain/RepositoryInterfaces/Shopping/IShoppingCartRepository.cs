using Explorer.Tours.Core.Domain.ShoppingCarts;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Shopping;

public interface IShoppingCartRepository
{
    List<ShoppingCart> GetAll();
    ShoppingCart GetByTourist(long touristId);
    ShoppingCart Create(ShoppingCart map);
    ShoppingCart Update(ShoppingCart map);
}
