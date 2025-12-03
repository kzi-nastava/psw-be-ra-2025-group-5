using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITourRepository
{
    PagedResult<Tour> GetPaged(int page, int pageSize);
    PagedResult<Tour> GetPagedByAuthor(long authorId, int page, int pageSize);
    List<Tour> GetAll();
    Tour Get(long id);
    Tour Create(Tour map);
    Tour Update(Tour map);
    void Delete(long id);
}