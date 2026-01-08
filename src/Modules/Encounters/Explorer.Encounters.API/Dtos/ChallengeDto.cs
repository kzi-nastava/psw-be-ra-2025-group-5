namespace Explorer.Encounters.API.Dtos;

public class ChallengeDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int ExperiencePoints { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public int? RequiredParticipants { get; set; }
    public int? RadiusInMeters { get; set; }
    public string? ImageUrl { get; set; }
}
