using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;
using Explorer.Tours.Core.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Tours;

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
        var query = _dbSet.Include(t => t.KeyPoints).Include(t => t.Durations).Include(t => t.Reviews).ThenInclude(r => r.Images).Include(t => t.RequiredEquipment);
        var task = query.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> GetPagedByAuthor(long authorId, int page, int pageSize)
    {
        var query = _dbSet
            .Include(t => t.KeyPoints).Include(t => t.Durations).Include(t => t.Reviews).ThenInclude(r => r.Images)
            .Include(t => t.RequiredEquipment)
            .Where(t => t.AuthorId == authorId);
        var task = query.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> SearchByLocation(double latitude, double longitude, double distanceKm, int page, int pageSize)
    {
        var allPublishedTours = _dbSet
            .Include(t => t.KeyPoints)
            .Where(t => t.Status == TourStatus.Published)
            .ToList();

        var filteredTours = allPublishedTours
            .Where(tour => tour.KeyPoints.Any(kp =>
            {
                var distance = GeographyHelper.CalculateDistance(
                    latitude, longitude,
                    kp.Location.Latitude, kp.Location.Longitude
                );
                return distance <= distanceKm;
            }))
            .ToList();

        var count = filteredTours.Count;

        if (pageSize != 0 && page != 0)
        {
            filteredTours = filteredTours
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        return new PagedResult<Tour>(filteredTours, count);
    }

    public List<Tour> GetAll()
    {
        return _dbSet.Include(t => t.KeyPoints).Include(t => t.Durations).Include(t => t.Reviews).ThenInclude(r => r.Images).Include(t => t.RequiredEquipment).ToList();
    }

    public Tour Get(long id)
    {
        var entity = _dbSet
            .Include(t => t.KeyPoints).Include(t => t.Durations).Include(t => t.Reviews).ThenInclude(r => r.Images)
            .Include(t => t.RequiredEquipment)
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
            .Include(t => t.KeyPoints).Include(t => t.Durations).Include(t => t.Reviews).ThenInclude(r => r.Images)
            .Include(t => t.RequiredEquipment)
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

        // Brisanje review-a
        var newReviewIds = entity.Reviews.Select(r => r.Id).Where(id => id != 0).ToHashSet();
        var reviewIdsToDelete = existingTour.Reviews
            .Where(r => r.Id != 0 && !newReviewIds.Contains(r.Id))
            .Select(r => r.Id)
            .ToList();

        if (reviewIdsToDelete.Any())
        {
            var reviewsDbSet = DbContext.Set<TourReview>();
            foreach (var reviewId in reviewIdsToDelete)
            {
                var reviewToDelete = reviewsDbSet
                    .Include(r => r.Images)
                    .FirstOrDefault(r => r.Id == reviewId);
                if (reviewToDelete != null)
                    reviewsDbSet.Remove(reviewToDelete);
            }
            DbContext.SaveChanges();
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

    public PagedResult<Tour> GetPagedByStatus(TourStatus status, int page, int pageSize)
    {
        var query = _dbSet
            .Include(t => t.KeyPoints)
            .Include(t => t.RequiredEquipment)
            .Where(t => t.Status == status);

        var task = query.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public TourReview AddReview(long tourId, int grade, string? comment, DateTime? reviewTime, double progress, long touristId, string username, List<ReviewImage>? images = null)
    {
        var tour = Get(tourId);

        // 2. Kreiraj review sa slikama ODMAH - koristite pravi konstruktor
        var review = new TourReview(grade, comment, reviewTime, progress, touristId, tourId, images, username);

        // 3. Dodaj review u turu
        tour.Reviews.Add(review);

        // 4. ISTO KAO U UpdateReview - koristi DbContext.Update i SaveChanges
        DbContext.Update(tour);
        DbContext.SaveChanges();

        return review;
    }

    public void UpdateReview(long tourId, long reviewId, int grade, string? comment, double progress, List<ReviewImage>? images = null)
    {
        var tour = Get(tourId);
        var review = tour.Reviews.First(r => r.Id == reviewId);

        // 1. Update osnovnih podataka
        review.UpdateGrade(grade);
        review.UpdateComment(comment);
        review.UpdatePercentage(progress);

        // 2. Ako postoje slike – zameni ih kompletno
        if (images != null)
        {
            review.ReplaceImages(
                images
                    .OrderBy(i => i.Order)
                    .Select((img, index) => new ReviewImage(review.Id, img.Data, img.ContentType, index)).ToList()
            );
        }

        DbContext.Update(tour);
        DbContext.SaveChanges();
    }

    public void RemoveReview(long tourId, long reviewId)
    {
        var tour = Get(tourId);
        tour.RemoveReview(reviewId);

        DbContext.Update(tour);
        DbContext.SaveChanges();
    }

    public void AddDuration(long tourId, TourDuration duration)
    {
        var tour = Get(tourId);
        tour.AddDuration(duration);

        DbContext.Update(tour);
        DbContext.SaveChanges();
    }

    public void RemoveDuration(long tourId, TourDuration duration)
    {
        var tour = Get(tourId);
        tour.RemoveDuration(duration);

        DbContext.Update(tour);
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
