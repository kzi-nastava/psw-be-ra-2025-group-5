using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourReviewRepository
    {
        PagedResult<TourReview> GetPaged(int page, int pageSize);
        PagedResult<TourReview> GetByTour(long tourId, int page, int pageSize);
        PagedResult<TourReview> GetByTourist(long touristId, int page, int pageSize);
        TourReview Get(int id);
        TourReview Create(TourReview newReview);
        TourReview Update(TourReview newReview);
        void Delete(int id);
    }
}
