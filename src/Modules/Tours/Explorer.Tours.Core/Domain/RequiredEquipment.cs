using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class RequiredEquipment : Entity
    {
        public long EquipmentId { get; private set; }

        private RequiredEquipment() { }

        public RequiredEquipment(long equipmentId)
        {
            if (equipmentId <= 0) throw new ArgumentException("Invalid equipment id.", nameof(equipmentId));
            EquipmentId = equipmentId;
        }
    }
}
