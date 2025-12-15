namespace Explorer.Tours.API.Internal.Statistics;

public interface ITourStatisticsDbRepository
{
    IReadOnlyCollection<TourStatisticsItemDto> GetPurchasedTours(long userId);
    IReadOnlyCollection<TourStatisticsItemDto> GetCompletedTours(long userId);
}
