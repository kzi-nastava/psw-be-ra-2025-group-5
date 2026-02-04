using Explorer.Payments.API.Dtos.Pricing;

namespace Explorer.Payments.API.Dtos.ShoppingCart;

public class OrderItemDto
{
    public long TourId { get; set; }
    public string TourName { get; set; }
    public TourPriceDto ItemPrice { get; set; } = new();
    public long? RecipientId { get; set; }  
    public string? GiftMessage { get; set; }

    public bool IsGift => RecipientId.HasValue;
}