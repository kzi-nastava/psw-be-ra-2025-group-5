using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class ShoppingCartDbRepository : IShoppingCartRepository
{
    protected readonly PaymentsContext DbContext;
    private readonly DbSet<ShoppingCart> _dbSet;

    public ShoppingCartDbRepository(PaymentsContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<ShoppingCart>();
    }

    public List<ShoppingCart> GetAll()
    {
        return _dbSet.ToList();
    }

    public ShoppingCart GetByTourist(long touristId)
    {
        var entity = _dbSet.FirstOrDefault(t => t.TouristId == touristId);
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
