using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public.Reporting
{
    public interface ITourProblemService
    {
        PagedResult<TourProblemDto> GetPaged(int page, int pageSize);
        TourProblemDto Create(TourProblemDto problem);
        TourProblemDto Update(TourProblemDto problem);
        void Delete(long id);
    }
}