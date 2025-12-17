using Explorer.Stakeholders.API.Internal.Statistics;

namespace Explorer.Stakeholders.API.Dtos;

public class ProfileDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Biography { get; set; }
    public string Motto { get; set; }

    public string? ProfileImageBase64 { get; set; }

    public TouristStatisticsDto Statistics { get; set; }
}
