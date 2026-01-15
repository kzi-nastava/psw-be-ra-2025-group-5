using Explorer.Payments.API.Dtos.Pricing;

namespace Explorer.Payments.API.Dtos.ShoppingCart;

public class OrderItemDto
{
    public long TourId { get; set; }
    public string TourName { get; set; }
    public TourPriceDto ItemPrice { get; set; } = new();
}
