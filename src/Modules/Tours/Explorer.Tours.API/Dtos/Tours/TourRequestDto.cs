using Explorer.Tours.API.Dtos.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos.Tours
{
    public class TourRequestDto
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public long AuthorId { get; set; }
        public string Difficulty { get; set; }
        public LocationDto Location { get; set; }
        public string Description { get; set; }
        public double MaxPrice { get; set; }
        public List<string> Tags { get; set; }
        public string Status { get; set; }
        public long? AcceptedTourId { get; set; }
    }
}
