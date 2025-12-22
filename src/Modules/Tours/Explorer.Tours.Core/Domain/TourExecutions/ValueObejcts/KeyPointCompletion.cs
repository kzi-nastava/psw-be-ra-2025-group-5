using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.TourExecutions.ValueObejcts
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
