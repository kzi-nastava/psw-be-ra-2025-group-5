using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.Monuments.ValueObjects
{
    public class MonumentLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public MonumentLocation() { }
        public MonumentLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
