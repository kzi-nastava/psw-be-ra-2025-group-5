namespace Explorer.Stakeholders.API.Dtos.Diaries;
public class DiaryDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? City { get; set; }
    public long TouristId { get; set; }
}
