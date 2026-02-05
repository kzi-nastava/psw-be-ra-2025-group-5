using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Payments.Core.Domain.Shared;

namespace Explorer.Payments.Core.Domain;

public class ShoppingCart : AggregateRoot
{
    public long TouristId { get; private set; }
    public List<OrderItem> Items { get; private set; } = [];
    public long? AppliedCouponId { get; private set; }

    private ShoppingCart() { }

    public ShoppingCart(long touristId)
    {
        Guard.AgainstZero(touristId, nameof(touristId));
        TouristId = touristId;
    }

    public void AddItem(long tourId, string tourName, double tourPrice, long? recipientId = null, string? giftMessage = null)
    {
        if (Items.Any(i => i.TourId == tourId && i.RecipientId == recipientId))
        {
            if (recipientId.HasValue)
                throw new InvalidOperationException($"Tour is already in the cart as a gift for this recipient.");
            else
                throw new InvalidOperationException("Tour is already in the cart.");
        }

        var orderItem = new OrderItem(tourId, tourName, tourPrice, recipientId, giftMessage);
        Items.Add(orderItem);
    }

    public void RemoveItem(long tourId, long? recipientId = null)
    {
        var item = Items.FirstOrDefault(i => i.TourId == tourId && i.RecipientId == recipientId);

        if (item is null)
            throw new InvalidOperationException("Item not found in the cart.");

        Items.Remove(item);
    }

    public void ApplyCoupon(long couponId)
    {
        AppliedCouponId = couponId;
    }

    public void ClearShoppingCart() => Items.Clear();
}