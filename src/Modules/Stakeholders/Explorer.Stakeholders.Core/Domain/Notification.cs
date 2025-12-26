using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public enum NotificationType
{
    ProblemReported,
    CommentAdded,
    DeadlineSet,
    TourClosed,
    ProblemStatusChanged,
    ClubInvite,
    ClubJoin
}

public class Notification : Entity
{
    public long UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public bool IsRead { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public long? TourProblemId { get; private set; }
    public long? TourId { get; private set; }
    public string? ActionUrl { get; private set; }
    public long? ClubId { get; private set; }

    private Notification() { }

    public Notification(
        long userId,
        NotificationType type,
        string title,
        string message,
        long? tourProblemId = null,
        long? tourId = null,
        string? actionUrl = null,
        long? clubId = null)
    {
        if (userId == 0) throw new ArgumentOutOfRangeException(nameof(userId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message is required.", nameof(message));

        UserId = userId;
        Type = type;
        Title = title;
        Message = message;
        IsRead = false;
        CreatedAt = DateTimeOffset.UtcNow;
        TourProblemId = tourProblemId;
        TourId = tourId;
        ActionUrl = actionUrl;
        ClubId = clubId;
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    public void MarkAsUnread()
    {
        IsRead = false;
    }
}

