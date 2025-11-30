using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class ShoppingCartDbRepository : IShoppingCartRepository
{
    protected readonly ToursContext DbContext;
    private readonly DbSet<ShoppingCart> _dbSet;

    public ShoppingCartDbRepository(ToursContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<ShoppingCart>();
    }

    public ShoppingCart GetByTourist(long touristId)
    {
        var entity = _dbSet.FirstOrDefault(t => t.TouristId == touristId);
        if (entity == null) throw new NotFoundException("Not found: " + touristId);
        return entity;
    }

    public ShoppingCart Create(ShoppingCart entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public ShoppingCart Update(ShoppingCart entity)
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
