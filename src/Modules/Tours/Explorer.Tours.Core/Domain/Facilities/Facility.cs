using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain.Facilities
{
    public class Facility : Entity
    {
        public enum FacilityType
        {
            wc, restaurant, parking, other
        }
        public string Name { get; init; }
        public double Latitude { get; init; } //Geografska širina
        public double Longitude { get; init; } //Geografska dužina
        public FacilityType Type { get; init; }

        public Facility(string name, double latitude, double longitude, FacilityType type)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Facility name cannot be null or empty.", nameof(name));

            if(latitude < -90 || latitude > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");

            if (longitude < -180 || longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");

            if (!Enum.IsDefined(typeof(FacilityType), type))
                throw new ArgumentException("Invalid facility type.", nameof(type));

            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Type = type;
        }
    }
}
