using Explorer.Tours.Core.Domain.TourPurchaseTokens;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Shopping;

public interface ITourPurchaseTokenRepository
{
    List<TourPurchaseToken> GetAll();
    TourPurchaseToken GetByTourAndTourist(long tourId, long touristId);
    TourPurchaseToken Create(TourPurchaseToken entity);
}
