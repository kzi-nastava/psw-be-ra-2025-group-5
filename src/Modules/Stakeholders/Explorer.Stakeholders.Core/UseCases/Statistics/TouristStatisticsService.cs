using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Tours.API.Internal;

namespace Explorer.Stakeholders.Core.UseCases.Statistics;

public class TouristStatisticsService : ITouristStatisticsService
{
    private readonly ITourStatisticsService _tourStatisticsProvider;

    public TouristStatisticsService(ITourStatisticsService tourStatisticsProvider)
    {
        _tourStatisticsProvider = tourStatisticsProvider;
    }

    public TouristStatisticsDto GetStatistics(long userId)
    {
        var purchasedTours = _tourStatisticsProvider.GetPurchasedToursCount(userId);
        var completedTours = _tourStatisticsProvider.GetCompletedTours(userId);

        return new TouristStatisticsDto
        {
            PurchasedToursCount = purchasedTours,
            CompletedToursCount = completedTours.Count,
            MostCommonTag = completedTours
                .SelectMany(t => t.Tags)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key,
            MostCommonDifficulty = completedTours
                .GroupBy(t => t.Difficulty)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key
        };
    }
}
