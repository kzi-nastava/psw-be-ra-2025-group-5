using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos.Tours.Reviews
{
    public class TourReviewDto
    {
        public long Id { get; set; }
        public int Grade { get; set; }
        public string? Comment { get; set; }
        public List<ReviewImageDto> Images { get; set; } = new();
        public DateTime? ReviewTime { get; set; }
        public double Progress { get; set; }
        public long TouristID { get; set; }
        public long TourID { get; set; }
        public string TouristUsername { get; set; }
        
    }
}
