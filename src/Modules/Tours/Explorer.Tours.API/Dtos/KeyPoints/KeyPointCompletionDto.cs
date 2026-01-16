using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos.KeyPoints
{
    public class KeyPointCompletionDto
    {
        public long KeyPointId { get; set; }
        public DateTime CompletedAt { get; set; }
        public double DistanceTravelled { get; set; }
    }
}
