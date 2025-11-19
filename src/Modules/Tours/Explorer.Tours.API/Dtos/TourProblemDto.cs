using System.ComponentModel.DataAnnotations;
using Explorer.Tours;

namespace Explorer.Tours.API.Dtos;

public class TourProblemDto
{
    public long Id { get; set; }

    [Range(1, long.MaxValue)]
    public long TourId { get; set; }

    [Range(1, long.MaxValue)]
    public long ReporterId { get; set; }

    [Required]
    public ProblemCategory Category { get; set; }

    [Required]
    public ProblemPriority Priority { get; set; }

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTimeOffset OccurredAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
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