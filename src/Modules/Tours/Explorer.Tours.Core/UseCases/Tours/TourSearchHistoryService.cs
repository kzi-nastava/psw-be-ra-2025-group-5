using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;

namespace Explorer.Tours.Core.UseCases.Tours;

public class TourSearchHistoryService : ITourSearchHistoryService
{
    private readonly ITourSearchHistoryRepository _repository;
    private readonly IMapper _mapper;

    public TourSearchHistoryService(ITourSearchHistoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public TourSearchHistoryDto SaveSearch(long userId, TourSearchDto searchDto)
    {
        var searchHistory = new TourSearchHistory(
            userId,
            searchDto.Latitude,
            searchDto.Longitude,
            searchDto.Distance,
            searchDto.Difficulty,
            searchDto.MinPrice,
            searchDto.MaxPrice,
            searchDto.Tags,
            searchDto.SortBy,
            searchDto.SortOrder
        );

        var result = _repository.Create(searchHistory);
        return _mapper.Map<TourSearchHistoryDto>(result);
    }

    public List<TourSearchHistoryDto> GetSearchHistory(long userId)
    {
        var history = _repository.GetByUser(userId);
        return history.Select(_mapper.Map<TourSearchHistoryDto>).ToList();
    }

    public List<string> GetMostFrequentTags(long userId, int topCount = 5)
    {
        var history = _repository.GetByUser(userId, 50);
        var allTags = history.SelectMany(h => h.Tags ?? new List<string>()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
        var grouped = allTags.GroupBy(t => t.ToLowerInvariant()).OrderByDescending(g => g.Count()).Take(topCount).ToList();
        return grouped.Select(g => g.First()).ToList();
    }

    public void DeleteSearch(long id)
    {
        _repository.Delete(id);
    }
}

