using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITouristEquipmentService
    {
        PagedResult<TouristEquipmentDto> GetPaged(int page, int pageSize);
        TouristEquipmentDto Create(TouristEquipmentDto touristEquipment);
        TouristEquipmentDto Update(TouristEquipmentDto touristEquipment);
        void Delete(long id);
    }
}
