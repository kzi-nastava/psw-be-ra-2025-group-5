using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public.Statistics;

public interface IAuthorStatisticsService
{
    AuthorStatisticsDto GetStatistics(long authorId);
}
