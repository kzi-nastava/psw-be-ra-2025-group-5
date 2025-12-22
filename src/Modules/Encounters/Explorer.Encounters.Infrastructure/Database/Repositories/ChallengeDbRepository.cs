using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database.Repositories;

public class ChallengeDbRepository : IChallengeRepository
{
    protected readonly EncountersContext DbContext;
    private readonly DbSet<Challenge> _dbSet;

    public ChallengeDbRepository(EncountersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<Challenge>();
    }

    public PagedResult<Challenge> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public Challenge Get(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new NotFoundException("Not found: " + id);
        return entity;
    }

    public Challenge Create(Challenge entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public Challenge Update(Challenge entity)
    {
        try
        {
            var existingChallenge = Get(entity.Id);
            existingChallenge.Update(
                entity.Name,
                entity.Description,
                entity.Latitude,
                entity.Longitude,
                entity.ExperiencePoints,
                entity.Status,
                entity.Type
            );
            DbContext.SaveChanges();
            return existingChallenge;
        }
        catch (DbUpdateException e)
        {
            throw new NotFoundException(e.Message);
        }
    }

    public void Delete(long id)
    {
        var entity = Get(id);
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}
