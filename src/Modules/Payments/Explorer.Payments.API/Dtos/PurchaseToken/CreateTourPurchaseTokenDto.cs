namespace Explorer.Payments.API.Dtos.PurchaseToken;

public class CreateTourPurchaseTokenDto
{
    public long TourId { get; set; }
    public long TouristId { get; set; }
    public bool IsFree { get; set; }
}
