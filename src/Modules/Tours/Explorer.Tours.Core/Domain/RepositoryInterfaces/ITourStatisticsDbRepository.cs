
namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITourStatisticsDbRepository
{
    IReadOnlyCollection<TourStatisticsItem> GetCompletedTours(long userId);
}
