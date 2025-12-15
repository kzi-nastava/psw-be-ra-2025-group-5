using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Stakeholders.API.Internal.Statistics;
using Explorer.Tours.API.Internal.Statistics;

namespace Explorer.Stakeholders.Core.UseCases.Statistics
{
    public class TouristStatisticsService : ITouristStatisticsService
    {
        private readonly ITourStatisticsDbRepository _tourStatisticsProvider;

        public TouristStatisticsService(ITourStatisticsDbRepository tourStatisticsProvider)
        {
            _tourStatisticsProvider = tourStatisticsProvider;
        }

        public API.Internal.Statistics.TouristStatisticsDto GetStatistics(long userId)
        {
            var purchasedTours = _tourStatisticsProvider.GetPurchasedTours(userId);
            var completedTours = _tourStatisticsProvider.GetCompletedTours(userId);

            return new API.Internal.Statistics.TouristStatisticsDto
            {
                PurchasedToursCount = purchasedTours.Count,
                CompletedToursCount = completedTours.Count,
                MostCommonTag = purchasedTours
                    .SelectMany(t => t.Tags)
                    .GroupBy(t => t)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key,
                MostCommonDifficulty = purchasedTours
                    .GroupBy(t => t.Difficulty)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key
            };
        }
    }
}
