using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;

namespace Explorer.Tours.Core.UseCases.Tours;

public class TourSuggestionService : ITourSuggestionService
{
    private const double DefaultLatitude = 44.7866;
    private const double DefaultLongitude = 20.4489;
    private const double DefaultDistance = 200;

    private readonly ITourSearchHistoryService _searchHistoryService;
    private readonly ITourService _tourService;
    private readonly ITouristPreferencesRepository _touristPreferencesRepository;

    public TourSuggestionService(ITourSearchHistoryService searchHistoryService, ITourService tourService, ITouristPreferencesRepository touristPreferencesRepository)
    {
        _searchHistoryService = searchHistoryService;
        _tourService = tourService;
        _touristPreferencesRepository = touristPreferencesRepository;
    }

    public PagedResult<TourDto> GetSuggestedTours(long userId, int page, int pageSize)
    {
        var searchDto = BuildSearchDto(userId);
        return _tourService.SearchByLocation(searchDto, page, pageSize);
    }

    private TourSearchDto BuildSearchDto(long userId)
    {
        var history = _searchHistoryService.GetSearchHistory(userId);
        var tags = _searchHistoryService.GetMostFrequentTags(userId, 5);

        if (!tags.Any())
        {
            var preferences = _touristPreferencesRepository.Get(userId);
            tags = preferences?.PreferredTags ?? new List<string>();
        }

        if (history.Any())
        {
            var lastSearch = history.First();
            return new TourSearchDto
            {
                Latitude = lastSearch.Latitude,
                Longitude = lastSearch.Longitude,
                Distance = lastSearch.Distance,
                Difficulty = lastSearch.Difficulty,
                MinPrice = lastSearch.MinPrice,
                MaxPrice = lastSearch.MaxPrice,
                Tags = tags.Any() ? tags : null,
                SortBy = lastSearch.SortBy,
                SortOrder = lastSearch.SortOrder
            };
        }

        return new TourSearchDto
        {
            Latitude = DefaultLatitude,
            Longitude = DefaultLongitude,
            Distance = DefaultDistance,
            Difficulty = null,
            MinPrice = null,
            MaxPrice = null,
            Tags = tags.Any() ? tags : null,
            SortBy = null,
            SortOrder = null
        };
    }
}
