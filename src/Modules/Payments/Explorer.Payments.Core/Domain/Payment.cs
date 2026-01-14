using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class Payment : AggregateRoot
    {
        public long TouristId { get; private set; }
        public long TourId { get; private set; }
        public double Price { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Payment() { }

        public Payment(long touristId, long tourId, double price)
        {
            TouristId = touristId;
            TourId = tourId;
            Price = price;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
