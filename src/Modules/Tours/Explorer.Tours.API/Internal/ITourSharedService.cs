using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;

namespace Explorer.Tours.API.Internal;

public interface ITourSharedService
{
    TourDto Get(long id);
    PagedResult<TourDto> GetPagedByAuthor(long authorId, int page, int pageSize);
    Dictionary<long, int> GetDurationsByTransport(IEnumerable<long> tourIds, string transportType);
}