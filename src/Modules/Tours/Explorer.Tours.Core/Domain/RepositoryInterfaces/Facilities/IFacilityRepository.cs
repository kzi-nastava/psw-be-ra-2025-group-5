using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.Facilities;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Facilities
{
    public interface IFacilityRepository
    {
        PagedResult<Facility> GetPaged(int page, int pageSize);
        Facility Create(Facility map);
        Facility Update(Facility map);
        void Delete(long id);
        IEnumerable<Facility> GetAllForTourists(); 
    }
}
