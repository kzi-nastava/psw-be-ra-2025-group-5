using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.TourExecutions.ValueObejcts;


namespace Explorer.Tours.Core.Domain.TourExecutions
{
    public enum TourExecutionStatus { Active, Completed, Abandoned }
    public class TourExecution : AggregateRoot
    {
        public long UserId { get; private set; }
        public long TourId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public TourExecutionStatus Status { get; private set; }
        public DateTime LastActivity { get; private set; }
        public int CurrentKeyPoint { get; private set; }

        private readonly List<KeyPointCompletion> _completedKeyPoints = new();
        public IReadOnlyList<KeyPointCompletion> CompletedKeyPoints => _completedKeyPoints.AsReadOnly();

        private TourExecution() { }
        private TourExecution(long userId, long tourId) 
        {
            UserId = userId;
            TourId = tourId;
            StartTime = DateTime.UtcNow;
            Status = TourExecutionStatus.Active;
            LastActivity = StartTime;
            CurrentKeyPoint = 0;
        }

        public static TourExecution StartNew(long userId, long tourId)
        {
            return new TourExecution(userId, tourId);
        }


        public void CompleteKeyPoint(long keyPointId, double distanceTravelled)
        {
            if (Status != TourExecutionStatus.Active)
                throw new InvalidOperationException("Cannot complete key point for inactive session.");

            _completedKeyPoints.Add(new KeyPointCompletion(keyPointId, DateTime.UtcNow, distanceTravelled));

            CurrentKeyPoint++;

            LastActivity = DateTime.UtcNow;
        }

        public void CompleteTour()
        {
            Status = TourExecutionStatus.Completed;
            EndTime = DateTime.UtcNow;
            LastActivity = EndTime.Value;
        }

        public void AbandonTour()
        {
            Status = TourExecutionStatus.Abandoned;
            EndTime = DateTime.UtcNow;
            LastActivity = EndTime.Value;
        }

        public void UpdateActivity()
        {
            LastActivity = DateTime.UtcNow;
        }


    }
}
