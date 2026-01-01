namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface IShoppingCartRepository
{
    List<ShoppingCart> GetAll();
    ShoppingCart GetByTourist(long touristId);
    ShoppingCart Create(ShoppingCart map);
    ShoppingCart Update(ShoppingCart map);
}
