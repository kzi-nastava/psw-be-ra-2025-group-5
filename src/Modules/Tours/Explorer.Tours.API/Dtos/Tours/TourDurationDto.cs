using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos.Tours
{
    public class TourDurationDto
    {
        public string TransportType { get; set; }
        public int DurationMinutes { get; set; }
    }
}
