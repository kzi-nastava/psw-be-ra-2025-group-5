using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourExecutionDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TourId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public DateTime LastActivity { get; set; }
        public List<KeyPointCompletionDto> CompletedKeyPoints { get; set; } = new();
        public KeyPointDto? NextKeyPoint { get; set; }
        public double PercentCompleted { get; set; }
    }
}
