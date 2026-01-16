using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Explorer.Tours.Core.Domain.Tours.ValueObjects
{

    public class TourDuration : ValueObject
    {
        public enum TransportTypeEnum { Walking, Bicycle, Car };
        public TransportTypeEnum TransportType { get; private set; }
        public int DurationMinutes { get; private set; }

        private TourDuration() { }

        public TourDuration(TransportTypeEnum type, int minutes)
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
