using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TouristEquipment : Entity
    {
        public int TouristId { get; init; }
        public int EquipmentId { get; init; }

        public TouristEquipment(int touristId, int equipmentId)
        {
            if (touristId == 0) throw new ArgumentException("Invalid TouristId.");
            if (equipmentId == 0) throw new ArgumentException("Invalid EquipmentId.");

            TouristId = touristId;
            EquipmentId = equipmentId;

        }
    }
}
