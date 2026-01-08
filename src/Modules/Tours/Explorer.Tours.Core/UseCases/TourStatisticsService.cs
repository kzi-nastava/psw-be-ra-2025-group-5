using AutoMapper;
using Explorer.Payments.API.Internal;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases;

public class TourStatisticsService : ITourStatisticsService
{
    private readonly ITourStatisticsDbRepository _StatisticsRepository;
    private readonly ITourPurchaseTokenSharedService _TokenService;
    private readonly IMapper _mapper;

    public TourStatisticsService(ITourStatisticsDbRepository statisticsRepository, IMapper mapper, ITourPurchaseTokenSharedService tokenService)
    {
        _StatisticsRepository = statisticsRepository;
        _mapper = mapper;
        _TokenService = tokenService;
    }

    public int GetPurchasedToursCount(long userId)
    {
        return _TokenService.GetByTourist(userId).Count;
    }

    public IReadOnlyCollection<TourStatisticsItemDto> GetCompletedTours(long userId)
    {
        var result = _StatisticsRepository.GetCompletedTours(userId);
        var tours = result.Select(_mapper.Map<TourStatisticsItemDto>).ToList();
        return tours;
    }
}
