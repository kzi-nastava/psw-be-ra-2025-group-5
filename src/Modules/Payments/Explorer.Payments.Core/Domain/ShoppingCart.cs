using Explorer.BuildingBlocks.Core.Domain;

using Explorer.Payments.Core.Domain.Shared;

namespace Explorer.Payments.Core.Domain;

public class ShoppingCart : AggregateRoot
{
    public long TouristId { get; private set; }
    public List<OrderItem> Items { get; private set; } = new List<OrderItem>();

    public ShoppingCart() { }

    public ShoppingCart(long touristId)
    {
        Guard.AgainstZero(touristId, nameof(touristId));

        TouristId = touristId;
    }

    public void AddItem(long tourId, string tourName, double tourPrice)
    {
        if (Items.SingleOrDefault(i => i.TourId == tourId) is not null)
            throw new InvalidOperationException("Tour is already in the cart.");

        var orderItem = new OrderItem(tourId, tourName, tourPrice);
        Items.Add(orderItem);
    }

    public void RemoveItem(long tourId)
    {
        if (Items.SingleOrDefault(i => i.TourId == tourId) is not OrderItem item)
            throw new InvalidOperationException("Item not found in the cart.");

        Items.Remove(item);
    }

    public void ClearShoppingCart() => Items.Clear();

    public double CalculateTotal() => Items.Sum(i => i.ItemPrice);
}
