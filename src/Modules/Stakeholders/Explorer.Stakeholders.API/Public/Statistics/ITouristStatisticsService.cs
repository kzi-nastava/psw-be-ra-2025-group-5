
using Explorer.Stakeholders.API.Internal.Statistics;

namespace Explorer.Stakeholders.API.Public.Statistics
{
    public interface ITouristStatisticsService
    {
        TouristStatisticsDto GetStatistics(long userId);
    }
}
