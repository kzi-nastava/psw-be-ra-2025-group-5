using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public enum TransportType { Walking, Bicycle, Car };

    public class TourDuration : ValueObject
    { 
        public TransportType TransportType { get; private set; }
        public int DurationMinutes { get; private set; }

        private TourDuration() { }

        public TourDuration(TransportType type, int minutes)
        {
            if (minutes <= 0)
                throw new ArgumentException("Duration must be positive.");

            TransportType = type;
            DurationMinutes = minutes;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TransportType;
            yield return DurationMinutes;
        }
    }
}
