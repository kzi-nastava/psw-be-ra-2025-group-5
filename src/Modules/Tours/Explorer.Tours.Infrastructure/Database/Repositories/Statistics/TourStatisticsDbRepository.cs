
using Explorer.Tours.Core.Domain.TourExecutions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Statistics;

public class TourStatisticsDbRepository : ITourStatisticsDbRepository
{
    private readonly ToursContext _dbContext;

    public TourStatisticsDbRepository(ToursContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyCollection<TourStatisticsItem> GetCompletedTours(long userId)
    {
        return (
            from exec in _dbContext.TourExecutions
            join tour in _dbContext.Tours on exec.TourId equals tour.Id
            where exec.UserId == userId
                  && exec.Status == TourExecutionStatus.Completed
            select new TourStatisticsItem(tour.Difficulty, tour.Tags.ToList())
        ).ToList();
    }

    public IReadOnlyCollection<ToursByPrice> GetToursCountByPrice(long userId)
    {
        var tours = _dbContext.Tours
            .Where(t => t.AuthorId == userId);

        var result = new List<ToursByPrice>
        {
            new ToursByPrice(
                tours.Count(t => t.Price >= 0 && t.Price <= 20),
                "0–20"
            ),
            new ToursByPrice(
                tours.Count(t => t.Price > 20 && t.Price <= 50),
                "20–50"
            ),
            new ToursByPrice(
                tours.Count(t => t.Price > 50 && t.Price <= 100),
                "50–100"
            ),
            new ToursByPrice(
                tours.Count(t => t.Price > 100),
                "100+"
            )
        };

        return result;
    }
}
