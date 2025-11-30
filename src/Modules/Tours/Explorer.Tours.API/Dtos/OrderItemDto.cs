
namespace Explorer.Tours.API.Dtos;

public class OrderItemDto
{
    public long Id { get; set; }
    public long TourId { get; set; }
    public string TourName { get; set; }
    public double ItemPrice { get; set; }
}
