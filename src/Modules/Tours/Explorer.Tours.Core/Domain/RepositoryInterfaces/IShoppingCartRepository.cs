
namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IShoppingCartRepository
{
    ShoppingCart GetByTourist(long touristId);
    ShoppingCart Create(ShoppingCart map);
    ShoppingCart Update(ShoppingCart map);
}
