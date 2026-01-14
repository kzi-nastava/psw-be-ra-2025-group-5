using Explorer.Payments.API.Dtos.PurchaseToken;

namespace Explorer.Payments.API.Public;

public interface ITourPurchaseTokenService
{
    List<TourPurchaseTokenDto> GetAll();
    TourPurchaseTokenDto GetByTourAndTourist(long tourId, long touristId);
    List<TourPurchaseTokenDto> GetByTourist(long touristId);
    TourPurchaseTokenDto Create(CreateTourPurchaseTokenDto entity);
}
