using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Payments.Core.Domain.Shared;

namespace Explorer.Payments.Core.Domain;

public class TourPurchaseToken : AggregateRoot
{
    public long TourId { get; private set; }
    public long TouristId { get; private set; }

    private TourPurchaseToken() { }

    public TourPurchaseToken(long tourId, long touristId)
    {
        Guard.AgainstZero(tourId, nameof(tourId));
        Guard.AgainstZero(touristId, nameof(touristId));

        TourId = tourId;
        TouristId = touristId;
    }
}
