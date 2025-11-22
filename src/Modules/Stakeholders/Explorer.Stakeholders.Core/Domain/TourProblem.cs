using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public enum ProblemCategory
{
    Safety,
    Navigation,
    Content,
    Accessibility,
    Other
}

public enum ProblemPriority
{
    Low,
    Medium,
    High,
    Critical
}

public class TourProblem : Entity
{
    public long TourId { get; }
    public long ReporterId { get; }

    public ProblemCategory Category { get; private set; }
    public ProblemPriority Priority { get; private set; }

    public string Description { get; private set; }
    public DateTimeOffset OccurredAt { get; }
    public DateTimeOffset CreatedAt { get; }

    private TourProblem() { }

    public TourProblem(
        long tourId,
        long reporterId,
        ProblemCategory category,
        ProblemPriority priority,
        string description,
        DateTimeOffset occurredAt,
        DateTimeOffset? createdAt = null)
    {
        if (tourId <= 0) throw new ArgumentOutOfRangeException(nameof(tourId));
        if (reporterId <= 0) throw new ArgumentOutOfRangeException(nameof(reporterId));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.", nameof(description));

        TourId = tourId;
        ReporterId = reporterId;
        Category = category;
        Priority = priority;
        Description = description.Trim();
        OccurredAt = occurredAt;
        CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
    }
}