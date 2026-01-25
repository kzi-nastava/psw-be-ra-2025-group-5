using System;

namespace Explorer.Payments.Core.Domain;

public class OrderItem
{
    public long TourId { get; set; }
    public string TourName { get; set; }
    public double ItemPrice { get; set; }
    public long? RecipientId { get; set; }  
    public string? GiftMessage { get; set; }

    public OrderItem() { }

    public OrderItem(long tourId, string tourName, double itemPrice, long? recipientId = null, string? giftMessage = null)
    {
        TourId = tourId;
        TourName = tourName;
        ItemPrice = itemPrice;
        RecipientId = recipientId;
        GiftMessage = giftMessage;
    }

    public bool IsGift => RecipientId.HasValue;

    public override bool Equals(object? obj)
    {
        if (obj is not OrderItem other) return false;
        return TourId == other.TourId && RecipientId == other.RecipientId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TourId, RecipientId);
    }
}