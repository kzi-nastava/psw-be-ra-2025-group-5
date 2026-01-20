namespace Explorer.Stakeholders.API.Dtos.Badges;

public class BadgeDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public int Rank { get; set; }
    public int Type { get; set; }
    public int RequiredValue { get; set; }
}
