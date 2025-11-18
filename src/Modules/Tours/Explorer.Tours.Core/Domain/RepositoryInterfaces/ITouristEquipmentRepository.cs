using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITouristEquipmentRepository
    {
        PagedResult<TouristEquipment> GetPaged(int page, int pageSize);
        TouristEquipment Create(TouristEquipment map);
        TouristEquipment Update(TouristEquipment map);
        void Delete(long id);
    }
}
