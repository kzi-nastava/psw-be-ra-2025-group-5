using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourDbRepository : ITourRepository
{
    protected readonly ToursContext DbContext;
    private readonly DbSet<Tour> _dbSet;

    public TourDbRepository(ToursContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<Tour>();
    }

    public PagedResult<Tour> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> GetPagedByAuthor(long authorId, int page, int pageSize)
    {
        var task = _dbSet.Where(t => t.AuthorId == authorId).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public List<Tour> GetAll()
    {
        return _dbSet.ToList();
    }


    public Tour Get(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new NotFoundException("Not found: " + id);
        return entity;
    }

    public Tour Create(Tour entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public Tour Update(Tour entity)
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

    public void Close(long tourId)
    {
        var tour = _dbSet.Find(tourId);
        if (tour == null)
            throw new NotFoundException($"Tour {tourId} not found");

        if (tour.Status == TourStatus.Closed)
            throw new InvalidOperationException("Tour is already closed");

        DbContext.Entry(tour).Property(t => t.Status).CurrentValue = TourStatus.Closed;

        DbContext.SaveChanges();
    }
}