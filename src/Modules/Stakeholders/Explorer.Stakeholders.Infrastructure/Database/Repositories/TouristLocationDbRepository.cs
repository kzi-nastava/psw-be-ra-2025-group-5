using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class TouristLocationDbRepository : ITouristLocationRepository
{
    protected readonly StakeholdersContext DbContext;
    private readonly DbSet<TouristLocation> _dbSet;

    public TouristLocationDbRepository(StakeholdersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TouristLocation>();
    }

    public List<TouristLocation> GetAll()
    {
        return _dbSet.ToList();
    }

    public TouristLocation? GetByTourist(long id)
    {
        var entity = _dbSet.FirstOrDefault(l => l.PersonId == id);
        return entity;
    }

    public TouristLocation Create(TouristLocation entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public TouristLocation Update(TouristLocation entity)
    {
        try
        {
            DbContext.Update(entity);
            DbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new NotFoundException(e.Message);
        }
        return entity;
    }
}