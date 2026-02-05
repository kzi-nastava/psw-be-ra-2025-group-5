using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class TourPurchaseTokenDbRepository : ITourPurchaseTokenRepository
{
    protected readonly PaymentsContext DbContext;
    private readonly DbSet<TourPurchaseToken> _dbSet;

    public TourPurchaseTokenDbRepository(PaymentsContext dbContext)
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

    public List<TourPurchaseToken> GetByTourist(long touristId)
    {
        var entities = _dbSet.Where(t => t.TouristId == touristId).ToList();
        return entities;
    }

    public List<TourPurchaseToken> GetByTour(long tourId)
    {
        var entities = _dbSet.Where(t => t.TourId == tourId).ToList();
        return entities;
    }

    public TourPurchaseToken Create(TourPurchaseToken entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }
}
