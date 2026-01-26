namespace Explorer.Stakeholders.API.Dtos.Users;

public class ProfileDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Biography { get; set; }
    public string Motto { get; set; }
    public string? ProfileImagePath { get; set; }

    public int Level { get; set; }
    public int ExperiencePoints { get; set; }
    public int XPForNextLevel { get; set; }
    public bool CanCreateChallenges { get; set; }

    public TouristStatisticsDto Statistics { get; set; }
    public AuthorStatisticsDto AuthorStatistics { get; set; }
}

