using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Internal;

public interface ITourStatisticsService
{
    int GetPurchasedToursCount(long userId);
    IReadOnlyCollection<TourStatisticsItemDto> GetCompletedTours(long userId);
    IReadOnlyCollection<ToursByPriceDto> GetToursCountByPrice(long userId);
}
