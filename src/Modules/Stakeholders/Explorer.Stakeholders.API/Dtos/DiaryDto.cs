namespace Explorer.Stakeholders.API.Dtos;
public class DiaryDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int Status { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? City { get; set; }
    public long TouristId { get; set; }
}
