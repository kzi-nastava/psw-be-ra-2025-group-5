using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Tours.API.Internal;
using Explorer.Payments.API.Internal;

namespace Explorer.Stakeholders.Core.UseCases.Statistics;

public class AuthorStatisticsService : IAuthorStatisticsService
{
    private readonly ITourSharedService _tourSharedService;
    private readonly ITourPurchaseTokenSharedService _purchaseTokenService;

    public AuthorStatisticsService(ITourSharedService tourSharedService, ITourPurchaseTokenSharedService purchaseTokenService)
    {
        _tourSharedService = tourSharedService;
        _purchaseTokenService = purchaseTokenService;
    }

    public AuthorStatisticsDto GetStatistics(long authorId)
    {
        // Uzmi sve ture autora
        var authorTours = _tourSharedService.GetPagedByAuthor(authorId, 1, int.MaxValue);
        
        // Broj publishovanih tura
        var publishedToursCount = authorTours.Results.Count(t => t.Status == "Published");
        
        // Broj prodanih tura - uzimamo sve purchase tokens za sve ture ovog autora
        var soldToursCount = 0;
        foreach (var tour in authorTours.Results)
        {
            var tokensForTour = _purchaseTokenService.GetByTour(tour.Id);
            soldToursCount += tokensForTour.Count;
        }

        return new AuthorStatisticsDto
        {
            PublishedToursCount = publishedToursCount,
            SoldToursCount = soldToursCount
        };
    }
}
