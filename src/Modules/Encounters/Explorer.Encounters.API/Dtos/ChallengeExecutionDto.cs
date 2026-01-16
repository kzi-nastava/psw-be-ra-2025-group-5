namespace Explorer.Encounters.API.Dtos;

public class ChallengeExecutionDto
{
    public long Id { get; set; }
    public long ChallengeId { get; set; }
    public long TouristId { get; set; }
    public string Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? AbandonedAt { get; set; }
}
