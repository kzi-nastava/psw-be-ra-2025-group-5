using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Payments.Core.Domain.Shared;

namespace Explorer.Payments.Core.Domain;

public class TourSale : Entity
{
    public List<long> TourIds { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public uint DiscountPercentage { get; private set; }

    private TourSale() { }

    public TourSale(List<long> tourIds, DateTime expirationDate, uint discount)
    {
        Guard.AgainstNullOrEmpty(tourIds, nameof(tourIds));
        Guard.AgainstOutOfRange(expirationDate, DateTime.UtcNow, DateTime.UtcNow.AddDays(14), nameof(expirationDate));
        Guard.AgainstOutOfRange(discount, 1u, 100u, nameof(discount));

        TourIds = tourIds;
        CreationDate = DateTime.UtcNow;
        ExpirationDate = expirationDate;
        DiscountPercentage = discount;
    }
}
