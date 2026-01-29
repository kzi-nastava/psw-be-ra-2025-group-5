using AutoMapper;
using Explorer.Payments.API.Internal;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Tours.Core.UseCases;

public class TourStatisticsService : ITourStatisticsService, ITourAnalyticsService
{
    private readonly ITourStatisticsDbRepository _StatisticsRepository;
    private readonly ITourPurchaseTokenSharedService _TokenService;
    private readonly IMapper _mapper;
    private readonly ITourRepository _TourRepository;
    private readonly IPremiumSharedService _premiumService;


    public TourStatisticsService(ITourStatisticsDbRepository statisticsRepository, IMapper mapper, ITourPurchaseTokenSharedService tokenService, ITourRepository tourRepository, IPremiumSharedService premiumService)
    {
        _StatisticsRepository = statisticsRepository;
        _mapper = mapper;
        _TokenService = tokenService;
        _TourRepository = tourRepository;
        _premiumService = premiumService;
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

    public IReadOnlyCollection<ToursByPriceDto> GetToursCountByPrice(long userId)
    {
        EnsurePremium(userId);


        var data = _StatisticsRepository.GetToursCountByPrice(userId);

        return data
            .Select(t => new ToursByPriceDto
            {
                Count = t.Count,
                PriceRange = t.PriceRange
            })
            .ToList()
            .AsReadOnly();
    }

    public TourReviewStatisticsDto GetReviewStatistics(long authorId, string period)
    {
        EnsurePremium(authorId);


        var pagedResult = _TourRepository.GetPagedByAuthor(authorId, 0, 10000);
        var tours = pagedResult.Results;

        var allReviews = tours.SelectMany(t => t.Reviews).ToList();

        if (period != "all")
        {
            DateTime periodStart = DateTime.UtcNow;

            if (period == "24h") periodStart = periodStart.AddHours(-24);
            else if (period == "week") periodStart = periodStart.AddDays(-7);
            else if (period == "month") periodStart = periodStart.AddMonths(-1);
            else if (period == "6months") periodStart = periodStart.AddMonths(-6);

            allReviews = allReviews.Where(r => r.ReviewTime >= periodStart).ToList();
        }

        var stats = new TourReviewStatisticsDto
        {
            Count1 = allReviews.Count(r => r.Grade == 1),
            Count2 = allReviews.Count(r => r.Grade == 2),
            Count3 = allReviews.Count(r => r.Grade == 3),
            Count4 = allReviews.Count(r => r.Grade == 4),
            Count5 = allReviews.Count(r => r.Grade == 5),
            TotalCount = allReviews.Count
        };

        if (stats.TotalCount > 0)
            stats.AverageGrade = Math.Round(allReviews.Average(r => r.Grade), 2);
        else
            stats.AverageGrade = 0;

        return stats;
    }

    private void EnsurePremium(long userId)
    {
        if (!_premiumService.IsPremium(userId))
            throw new UnauthorizedAccessException("Premium required.");
    }
}
