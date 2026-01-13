using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.Equipments.Entities
{
    public class TouristEquipment : Entity
    {
        public long TouristId { get; init; }
        public long EquipmentId { get; init; }

        public TouristEquipment(long touristId, long equipmentId)
        {
            if (touristId == 0) throw new ArgumentException("Invalid TouristId.");
            if (equipmentId == 0) throw new ArgumentException("Invalid EquipmentId.");

            TouristId = touristId;
            EquipmentId = equipmentId;

        }
    }
}
