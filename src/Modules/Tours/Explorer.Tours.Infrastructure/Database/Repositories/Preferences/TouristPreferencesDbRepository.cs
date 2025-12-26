using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.Core.Domain.Preferences;
using Microsoft.EntityFrameworkCore;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Preferences;

public class TouristPreferencesDbRepository : ITouristPreferencesRepository
{
    protected readonly ToursContext DbContext;
    private readonly DbSet<TouristPreferences> _dbSet;

    public TouristPreferencesDbRepository(ToursContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TouristPreferences>();
    }

    public TouristPreferences? Get(long userId)
    {
        return _dbSet.FirstOrDefault(tp => tp.UserId == userId);
    }

    public TouristPreferences Create(TouristPreferences entity)
    {
        if (_dbSet.Any(tp => tp.UserId == entity.UserId))
            throw new InvalidOperationException("Preferences already exist for this user.");

        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public TouristPreferences Update(TouristPreferences updatedEntity)
    {
        var existingEntity = _dbSet.FirstOrDefault(tp => tp.UserId == updatedEntity.UserId);
        if (existingEntity == null) throw new NotFoundException("Not found: " + updatedEntity.UserId);

        existingEntity.Update(
            updatedEntity.PreferredDifficulty,
            updatedEntity.TransportationRatings,
            updatedEntity.PreferredTags
        );

        DbContext.SaveChanges();
        return existingEntity;
    }

    public void Delete(long userId)
    {
        var entity = Get(userId);
        if (entity == null) throw new NotFoundException("Not found: " + userId);
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}
