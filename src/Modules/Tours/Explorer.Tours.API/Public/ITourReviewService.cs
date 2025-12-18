using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public
{
    public interface ITourReviewService
    {
        PagedResult<TourReviewDto> GetPaged(int page, int pageSize);
        PagedResult<TourReviewDto> GetByTour(long tourId, int page, int pageSize);
        PagedResult<TourReviewDto> GetByTourist(long touristId, int page, int pageSize);
        TourReviewDto Get(long id);
        TourReviewDto Create(TourReviewDto dto);
        TourReviewDto Update(long id, TourReviewDto dto);
        void Delete(long id);
    }
}
