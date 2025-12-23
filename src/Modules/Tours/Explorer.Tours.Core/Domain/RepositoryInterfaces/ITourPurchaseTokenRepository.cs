
namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITourPurchaseTokenRepository
{
    List<TourPurchaseToken> GetAll();
    TourPurchaseToken GetByTourAndTourist(long tourId, long touristId);
    TourPurchaseToken Create(TourPurchaseToken entity);
    List<TourPurchaseToken> GetByTourist(long touristId); 

}
