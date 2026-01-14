using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public.Tour;

public interface ITourSearchHistoryService
{
    TourSearchHistoryDto SaveSearch(long userId, TourSearchDto searchDto);
    List<TourSearchHistoryDto> GetSearchHistory(long userId);
    void DeleteSearch(long id);
}

