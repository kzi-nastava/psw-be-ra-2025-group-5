using System.ComponentModel.DataAnnotations;
using Explorer.Stakeholders;

namespace Explorer.Stakeholders.API.Dtos;

public class TourProblemDto
{
    [Required]
    public long Id { get; set; }

    [Required]
    [Range(1, long.MaxValue)]
    public long TourId { get; set; }

    [Required]
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
    [Required]
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