namespace Explorer.Tours.API.Dtos.Shoppings;

public class ShoppingCartDto
{
    public long Id { get; set; }
    public long TouristId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public double Total => Items.Sum(i => i.ItemPrice);
}
