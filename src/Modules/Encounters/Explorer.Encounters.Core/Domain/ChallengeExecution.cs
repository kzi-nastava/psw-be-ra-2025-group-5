using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain;

public class ChallengeExecution : Entity
{
    public long ChallengeId { get; private set; }
    public long TouristId { get; private set; }
    public ChallengeExecutionStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? AbandonedAt { get; private set; }

    public ChallengeExecution(long challengeId, long touristId)
    {
        //if (challengeId <= 0) throw new ArgumentException("Invalid ChallengeId.");
        //if (touristId <= 0) throw new ArgumentException("Invalid TouristId.");

        ChallengeId = challengeId;
        TouristId = touristId;
        Status = ChallengeExecutionStatus.InProgress;
        StartedAt = DateTime.UtcNow;
        CompletedAt = null;
        AbandonedAt = null;
    }

    public void Complete()
    {
        if (Status != ChallengeExecutionStatus.InProgress)
            throw new InvalidOperationException("Can only complete a challenge that is in progress.");

        Status = ChallengeExecutionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Abandon()
    {
        if (Status != ChallengeExecutionStatus.InProgress)
            throw new InvalidOperationException("Can only abandon a challenge that is in progress.");

        Status = ChallengeExecutionStatus.Abandoned;
        AbandonedAt = DateTime.UtcNow;
    }
}

public enum ChallengeExecutionStatus
{
    InProgress,
    Completed,
    Abandoned
}
