
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public.Shopping;

public interface ITourPurchaseTokenService
{
    List<TourPurchaseTokenDto> GetAll();
    TourPurchaseTokenDto GetByTourAndTourist(long tourId, long touristId);
    TourPurchaseTokenDto Create(CreateTourPurchaseTokenDto entity);
    List<TourPurchaseTokenDto> GetByTourist(long touristId);
}
