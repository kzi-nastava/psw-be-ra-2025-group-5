namespace Explorer.Tours.API.Dtos;

public class TourStatisticsItemDto
{
    public string Difficulty { get; set; } = string.Empty;
    public IReadOnlyCollection<string> Tags { get; set; } = Array.Empty<string>();
}
