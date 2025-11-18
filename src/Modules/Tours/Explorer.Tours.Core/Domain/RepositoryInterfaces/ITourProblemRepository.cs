using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourProblemRepository
    {
        PagedResult<TourProblem> GetPaged(int page, int pageSize);
        TourProblem Create(TourProblem entity);
        TourProblem Update(TourProblem entity);
        void Delete(long id);
    }
}