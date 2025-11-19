using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class TouristPreferencesDbRepository : ITouristPreferencesRepository
{
    protected readonly StakeholdersContext DbContext;
    private readonly DbSet<TouristPreferences> _dbSet;

    public TouristPreferencesDbRepository(StakeholdersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TouristPreferences>();
    }

    //public TouristPreferences Get(long userId)
    //{
    //    var entity = _dbSet.FirstOrDefault(tp => tp.UserId == userId);
    //    if (entity == null) throw new NotFoundException("Not found: " + userId);
    //    return entity;
    //}

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