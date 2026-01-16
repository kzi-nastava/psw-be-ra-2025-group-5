namespace Explorer.Payments.API.Dtos.Pricing;

public class CreateTourSaleDto
{
    public long AuthorId { get; set; }
    public List<long> TourIds { get; set; } = [];
    public DateTime ExpirationDate { get; set; }
    public uint DiscountPercentage { get; set; }
}
