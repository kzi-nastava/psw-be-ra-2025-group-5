using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Tours;

public class TourSponsorshipDbRepository : ITourSponsorshipRepository
{
    private readonly ToursContext _dbContext;
    private readonly DbSet<TourSponsorship> _dbSet;

    public TourSponsorshipDbRepository(ToursContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TourSponsorship>();
    }

    public TourSponsorship Create(TourSponsorship sponsorship)
    {
        _dbSet.Add(sponsorship);
        _dbContext.SaveChanges();
        return sponsorship;
    }

    public TourSponsorship? GetActiveByTourId(long tourId)
    {
        return _dbSet
            .Where(s => s.TourId == tourId && s.EndDate > DateTime.UtcNow)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();
    }

    public List<long> GetActiveSponsoredTourIds()
    {
        return _dbSet
            .Where(s => s.EndDate > DateTime.UtcNow)
            .Select(s => s.TourId)
            .Distinct()
            .ToList();
    }

    public List<TourSponsorship> GetByAuthorId(long authorId)
    {
        return _dbSet
            .Where(s => s.AuthorId == authorId)
            .OrderByDescending(s => s.StartDate)
            .ToList();
    }
}
