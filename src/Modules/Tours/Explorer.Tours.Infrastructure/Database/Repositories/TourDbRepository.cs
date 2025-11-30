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
        var query = _dbSet.Include(t => t.KeyPoints);
        var task = query.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> GetPagedByAuthor(long authorId, int page, int pageSize)
    {
        var query = _dbSet
            .Include(t => t.KeyPoints)
            .Where(t => t.AuthorId == authorId);
        var task = query.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public List<Tour> GetAll()
    {
        return _dbSet.Include(t => t.KeyPoints).ToList();
    }

    public Tour Get(long id)
    {
        var entity = _dbSet
            .Include(t => t.KeyPoints)
            .FirstOrDefault(t => t.Id == id);
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
        // 1. Učitaj postojeći tour SA TRACKING-om
        var existingTour = _dbSet
            .Include(t => t.KeyPoints)
            .AsNoTracking() // ← Ne pratimo promene za sada
            .FirstOrDefault(t => t.Id == entity.Id);

        if (existingTour == null)
            throw new NotFoundException($"Tour with ID {entity.Id} not found.");

        // 2. Pronađi KeyPoints koji treba da se fizički obrišu iz baze
        var newKeyPointIds = entity.KeyPoints.Select(kp => kp.Id).Where(id => id != 0).ToHashSet();
        var keyPointIdsToDelete = existingTour.KeyPoints
            .Where(kp => kp.Id != 0 && !newKeyPointIds.Contains(kp.Id))
            .Select(kp => kp.Id)
            .ToList();

        Console.WriteLine($"[TourDbRepository] Existing KeyPoints: {string.Join(", ", existingTour.KeyPoints.Select(kp => kp.Id))}");
        Console.WriteLine($"[TourDbRepository] New KeyPoints: {string.Join(", ", newKeyPointIds)}");
        Console.WriteLine($"[TourDbRepository] KeyPoints to DELETE: {string.Join(", ", keyPointIdsToDelete)}");

        // 3. DIREKTNO obriši KeyPoints iz baze pomoću SQL-a
        if (keyPointIdsToDelete.Any())
        {
            var keyPointsDbSet = DbContext.Set<KeyPoint>();
            foreach (var keyPointId in keyPointIdsToDelete)
            {
                var keyPointToDelete = keyPointsDbSet.Find(keyPointId);
                if (keyPointToDelete != null)
                {
                    Console.WriteLine($"[TourDbRepository] Deleting KeyPoint ID: {keyPointId}");
                    keyPointsDbSet.Remove(keyPointToDelete);
                }
                else
                {
                    Console.WriteLine($"[TourDbRepository] KeyPoint ID {keyPointId} not found!");
                }
            }
            var deletedCount = DbContext.SaveChanges();
            Console.WriteLine($"[TourDbRepository] Deleted {deletedCount} KeyPoint(s) from database");
        }

        // 4. Attach i ažuriraj Tour
        DbContext.Update(entity);
        var updatedCount = DbContext.SaveChanges();
        Console.WriteLine($"[TourDbRepository] Updated Tour, affected rows: {updatedCount}");
        
        // 5. Vrati ažurirani entitet
        return Get(entity.Id);
    }

    public void Delete(long id)
    {
        var entity = Get(id);
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}