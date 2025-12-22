using Explorer.Tours.Core.Domain.RepositoryInterfaces.Shopping;
using Explorer.Tours.Core.Domain.TourPurchaseTokens;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Tours.PurchaseTokens;

public class TourPurchaseTokenDbRepository : ITourPurchaseTokenRepository
{
    protected readonly ToursContext DbContext;
    private readonly DbSet<TourPurchaseToken> _dbSet;

    public TourPurchaseTokenDbRepository(ToursContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TourPurchaseToken>();
    }

    public List<TourPurchaseToken> GetAll()
    {
        return _dbSet.ToList();
    }

    public TourPurchaseToken GetByTourAndTourist(long tourId, long touristId)
    {
        var entity = _dbSet.FirstOrDefault(t => t.TouristId == touristId && t.TourId == tourId);
        return entity;
    }

    public TourPurchaseToken Create(TourPurchaseToken entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }
}
