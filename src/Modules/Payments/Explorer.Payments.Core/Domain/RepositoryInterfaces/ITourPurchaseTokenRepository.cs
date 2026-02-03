
using Explorer.Payments.API.Dtos;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface ITourPurchaseTokenRepository
{
    List<TourPurchaseToken> GetAll();
    TourPurchaseToken GetByTourAndTourist(long tourId, long touristId);
    List<TourPurchaseToken> GetByTourist(long touristId);
    List<TourPurchaseToken> GetByTour(long tourId);
    TourPurchaseToken Create(TourPurchaseToken entity);
}
