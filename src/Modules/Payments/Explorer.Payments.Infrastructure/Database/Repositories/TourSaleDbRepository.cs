using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class TourSaleDbRepository : ITourSaleRepository
{
    protected readonly PaymentsContext DbContext;
    private readonly DbSet<TourSale> _dbSet;

    public TourSaleDbRepository(PaymentsContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TourSale>();
    }

    public List<TourSale> GetAll() => [.. _dbSet];

    public TourSale? Get(long id)
    {
        var entity = _dbSet.FirstOrDefault(sale => sale.Id == id);
        return entity ?? throw new NotFoundException("Not found: " + id);
    }

    public TourSale? GetActiveSaleForTour(long tourId) => _dbSet.AsEnumerable().FirstOrDefault(sale => DateTime.UtcNow < sale.ExpirationDate && sale.TourIds.Contains(tourId));

    public List<TourSale> GetByAuthor(long authorId, bool onlyActive)
    => onlyActive 
        ? [.. _dbSet.Where(sale => sale.AuthorId == authorId && DateTime.UtcNow < sale.ExpirationDate)]
        : [.. _dbSet.Where(sale => sale.AuthorId == authorId)];

    public TourSale Create(TourSale entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public TourSale Update(TourSale entity)
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

    public void Delete(long id)
    {
        var entity = Get(id);
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}
