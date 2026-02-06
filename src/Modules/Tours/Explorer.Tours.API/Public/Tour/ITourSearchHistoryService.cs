using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public.Tour;

public interface ITourSearchHistoryService
{
    TourSearchHistoryDto SaveSearch(long userId, TourSearchDto searchDto);
    List<TourSearchHistoryDto> GetSearchHistory(long userId);
    List<string> GetMostFrequentTags(long userId, int topCount = 5);
    void DeleteSearch(long id);
}

