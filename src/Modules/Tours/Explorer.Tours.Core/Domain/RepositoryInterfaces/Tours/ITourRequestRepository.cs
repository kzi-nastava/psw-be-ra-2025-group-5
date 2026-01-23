using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.Tours.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours
{
    public interface ITourRequestRepository
    {
        TourRequest Create(TourRequest request);
        TourRequest Update(TourRequest request);
        PagedResult<TourRequest> GetPagedByTourist(long touristId, int page, int pageSize);
        PagedResult<TourRequest> GetPagedByAuthor(long authorId, int page, int pageSize);
        TourRequest Get(long id);
    }
}
