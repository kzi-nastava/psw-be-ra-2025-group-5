using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Tours;

public class TourSearchHistoryDbRepository : ITourSearchHistoryRepository
{
    protected readonly ToursContext DbContext;
    private readonly DbSet<TourSearchHistory> _dbSet;

    public TourSearchHistoryDbRepository(ToursContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TourSearchHistory>();
    }

    public TourSearchHistory Create(TourSearchHistory searchHistory)
    {
        _dbSet.Add(searchHistory);
        DbContext.SaveChanges();
        return searchHistory;
    }

    public List<TourSearchHistory> GetByUser(long userId)
    {
        return _dbSet
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.CreatedAt)
            .ToList();
    }

    public void Delete(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new NotFoundException("Not found: " + id);
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}

