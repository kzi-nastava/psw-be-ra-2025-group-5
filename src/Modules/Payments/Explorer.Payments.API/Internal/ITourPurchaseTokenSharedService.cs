using Explorer.Payments.API.Dtos;

namespace Explorer.Payments.API.Internal;

public interface ITourPurchaseTokenSharedService
{
    TourPurchaseTokenDto GetByTourAndTourist(long tourId, long touristId);
    List<TourPurchaseTokenDto> GetByTourist(long touristId);
}
