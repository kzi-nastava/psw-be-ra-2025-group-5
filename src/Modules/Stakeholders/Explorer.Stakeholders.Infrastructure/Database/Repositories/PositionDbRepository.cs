using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class PositionDbRepository : IPositionRepository
{
    protected readonly StakeholdersContext DbContext;
    private readonly DbSet<Position> _dbSet;

    public PositionDbRepository(StakeholdersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<Position>();
    }

    public List<Position> GetAll()
    {
        return _dbSet.ToList();
    }

    public Position? GetByTourist(long id)
    {
        var entity = _dbSet.FirstOrDefault(l => l.PersonId == id);
        return entity;
    }

    public Position Create(Position entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public Position Update(Position entity)
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