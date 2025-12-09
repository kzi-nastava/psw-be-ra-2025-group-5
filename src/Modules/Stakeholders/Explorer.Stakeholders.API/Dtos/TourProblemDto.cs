using System.ComponentModel.DataAnnotations;
using Explorer.Stakeholders;

namespace Explorer.Stakeholders.API.Dtos;

public class TourProblemDto
{
    public long Id { get; set; }

    [NotDefault]
    public long TourId { get; set; }

    [NotDefault]
    public long ReporterId { get; set; }

    public ProblemCategory Category { get; set; }

    public ProblemPriority Priority { get; set; }

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public DateTimeOffset OccurredAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
    public bool IsResolved { get; set; }
}

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