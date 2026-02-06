using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;

namespace Explorer.Tours.API.Public.Tour;

public interface ITourSuggestionService
{
    PagedResult<TourDto> GetSuggestedTours(long userId, int page, int pageSize);
}
