
namespace Explorer.Payments.API.Dtos;

public class OrderItemDto
{
    public long TourId { get; set; }
    public string TourName { get; set; }
    public double ItemPrice { get; set; }
}
