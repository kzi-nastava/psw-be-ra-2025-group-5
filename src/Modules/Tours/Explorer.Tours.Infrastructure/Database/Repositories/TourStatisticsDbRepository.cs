using Explorer.Tours.API.Internal.Statistics;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourStatisticsDbRepository : ITourStatisticsDbRepository
{

    private readonly ToursContext _dbContext;

    public TourStatisticsDbRepository(ToursContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyCollection<TourStatisticsItemDto> GetPurchasedTours(long userId)
    {
        return (from token in _dbContext.TourPurchaseTokens
                join tour in _dbContext.Tours on token.TourId equals tour.Id
                where token.TouristId == userId
                select new TourStatisticsItemDto
                {
                    Difficulty = tour.Difficulty.ToString(),
                    Tags = tour.Tags.ToList()
                }).ToList();
    }
        
    public IReadOnlyCollection<TourStatisticsItemDto> GetCompletedTours(long userId)
    {
        return (from exec in _dbContext.TourExecutions
                join tour in _dbContext.Tours on exec.TourId equals tour.Id
                where exec.UserId == userId && exec.Status == TourExecutionStatus.Completed
                select new TourStatisticsItemDto
                {
                    Difficulty = tour.Difficulty.ToString(),
                    Tags = tour.Tags.ToList()
                }).ToList();
    }

    public TouristStatisticsDto GetStatistics(long userId)
    {
        var purchasedTours = (from token in _dbContext.TourPurchaseTokens
                              join tour in _dbContext.Tours on token.TourId equals tour.Id
                              where token.TouristId == userId
                              select tour)
                     .ToList();


        var completedTours = _dbContext.TourExecutions
            .Where(te => te.UserId == userId && te.Status == Core.Domain.TourExecutionStatus.Completed)
            .ToList();

        int purchasedCount = purchasedTours.Count;
        int completedCount = completedTours.Count;

        var mostCommonDifficulty = purchasedTours
            .GroupBy(t => t.Difficulty)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key;

        var mostCommonTag = purchasedTours
            .SelectMany(t => t.Tags)
            .GroupBy(tag => tag)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key;

        return new TouristStatisticsDto
        {
            PurchasedToursCount = purchasedCount,
            CompletedToursCount = completedCount,
            MostCommonDifficulty = mostCommonDifficulty?.ToString(),
            MostCommonTag = mostCommonTag
        };
    }

} 
