using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.Tours.Entities
{
    public class CheckProximityResult
    {
        public bool IsNearKeyPoint { get; }
        public long? CompletedKeyPointId { get; }
        public DateTime? CompletedAt { get; }
        public bool UnlockedSecret { get; }
        public double PercentCompleted { get; }
        public DateTime LastActivity { get; }
        public KeyPoint? NextKeyPoint { get; }

        public CheckProximityResult(bool isNearKeyPoint, long? completedKeyPointId, DateTime? completedAt, bool unlockedSecret, double percentCompleted, DateTime lastActivity, KeyPoint? nextKeyPoint)
        {
            IsNearKeyPoint = isNearKeyPoint;
            CompletedKeyPointId = completedKeyPointId;
            CompletedAt = completedAt;
            UnlockedSecret = unlockedSecret;
            PercentCompleted = percentCompleted;
            LastActivity = lastActivity;
            NextKeyPoint = nextKeyPoint;
        }
    }
}
