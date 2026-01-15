using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class BundleItem : Entity
    {
        public long BundleId { get; private set; }
        public long TourId { get; private set; }

        public BundleItem(long bundleId, long tourId)
        {
            BundleId = bundleId;
            TourId = tourId;
        }
    }
}
