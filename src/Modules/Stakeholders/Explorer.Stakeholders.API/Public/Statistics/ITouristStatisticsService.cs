using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public.Statistics;

public interface ITouristStatisticsService
{
    TouristStatisticsDto GetStatistics(long userId);
}
