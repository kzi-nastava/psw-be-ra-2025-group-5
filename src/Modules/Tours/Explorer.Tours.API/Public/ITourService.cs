using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public;

public interface ITourService
{
    PagedResult<TourDto> GetPaged(int page, int pageSize);
    PagedResult<TourDto> GetPagedByAuthor(long authorId, int page, int pageSize);
    List<string> GetAllTags();
    TourDto Create(TourDto Tour);
    TourDto Update(TourDto Tour);
    void Delete(long id);
    void CloseTour(long tourId);
}