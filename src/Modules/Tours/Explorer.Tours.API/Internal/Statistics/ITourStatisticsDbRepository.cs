namespace Explorer.Tours.API.Internal.Statistics;

public interface ITourStatisticsDbRepository
{
    int GetPurchasedToursCount(long userId);
    IReadOnlyCollection<TourStatisticsItemDto> GetCompletedTours(long userId);
}
