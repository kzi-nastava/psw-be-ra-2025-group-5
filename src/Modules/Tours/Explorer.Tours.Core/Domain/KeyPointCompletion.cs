using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain; 

namespace Explorer.Tours.Core.Domain
{
    public class KeyPointCompletion : ValueObject
    {
        public long KeyPointId { get; }
        public DateTime CompletedAt { get; }
        public double DistanceTravelled { get; }

        public KeyPointCompletion(long keyPointId, DateTime completedAt, double distanceTravelled)
        {
            KeyPointId = keyPointId;
            CompletedAt = completedAt;
            DistanceTravelled = distanceTravelled;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return KeyPointId;
            yield return CompletedAt;
        }
    }
}
