using Explorer.Payments.API.Dtos.PurchaseToken;

namespace Explorer.Payments.API.Internal;

public interface ITourPurchaseTokenSharedService
{
    TourPurchaseTokenDto GetByTourAndTourist(long tourId, long touristId);
    List<TourPurchaseTokenDto> GetByTourist(long touristId);
    List<TourPurchaseTokenDto> GetByTour(long tourId);
}
