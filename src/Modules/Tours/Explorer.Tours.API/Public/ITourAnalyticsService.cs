using Explorer.Tours.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public
{
    public interface ITourAnalyticsService
    {
        IReadOnlyCollection<ToursByPriceDto> GetToursCountByPrice(long userId);
        TourReviewStatisticsDto GetReviewStatistics(long authorId, string period);
    }
}
