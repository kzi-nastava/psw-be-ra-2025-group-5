using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class CheckProximityDto
    {
        
           public bool IsNearKeyPoint { get; set; }
           public long? CompletedKeyPointId { get; set; }
           public DateTime? CompletedAt { get; set; }
           public bool UnlockedSecret { get; set; }
           public string? SecretText { get; set; }
           public double PercentCompleted { get; set; } 
           public DateTime LastActivity { get; set; }
           public KeyPointDto? NextKeyPoint { get; set; }

    }
}
