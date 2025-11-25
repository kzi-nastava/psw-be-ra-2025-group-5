using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public.Reporting
{
    public interface ITourProblemService
    {
        PagedResult<TourProblemDto> GetPaged(int page, int pageSize);
        TourProblemDto Create(TourProblemDto problem);
        TourProblemDto Update(TourProblemDto problem);
        void Delete(long id);
    }
}