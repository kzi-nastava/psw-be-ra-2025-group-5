using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.TourPurchaseTokens;

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
