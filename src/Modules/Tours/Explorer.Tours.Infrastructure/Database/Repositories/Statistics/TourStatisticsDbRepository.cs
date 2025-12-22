using Explorer.Tours.API.Internal.Statistics;
using Explorer.Tours.Core.Domain.TourExecutions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Statistics;

public class TourStatisticsDbRepository : ITourStatisticsDbRepository
{

    private readonly ToursContext _dbContext;

    public TourStatisticsDbRepository(ToursContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int GetPurchasedToursCount(long userId)
    {
        return _dbContext.TourPurchaseTokens
            .Count(t => t.TouristId == userId);
    }

    public IReadOnlyCollection<TourStatisticsItemDto> GetCompletedTours(long userId)
    {
        return (
            from exec in _dbContext.TourExecutions
            join tour in _dbContext.Tours on exec.TourId equals tour.Id
            where exec.UserId == userId
                  && exec.Status == TourExecutionStatus.Completed
            select new TourStatisticsItemDto
            {
                Difficulty = tour.Difficulty.ToString(),
                Tags = tour.Tags.ToList()
            }
        ).ToList();
    }
}
