namespace Explorer.Stakeholders.API.Dtos;

public class NotificationDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long? TourProblemId { get; set; }
    public long? TourId { get; set; }
    public string? ActionUrl { get; set; }
    public long? ClubId { get; set; }
}

