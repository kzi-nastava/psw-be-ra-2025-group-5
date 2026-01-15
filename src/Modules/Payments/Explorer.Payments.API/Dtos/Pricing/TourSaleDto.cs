namespace Explorer.Payments.API.Dtos.Pricing;

public class TourSaleDto
{
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public List<long> TourIds { get; set; } = [];
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public uint DiscountPercentage { get; set; }
    public bool IsActive => DateTime.UtcNow < ExpirationDate;
}
