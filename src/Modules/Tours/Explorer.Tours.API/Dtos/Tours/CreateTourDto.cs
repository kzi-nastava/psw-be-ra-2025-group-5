using Explorer.Tours.API.Dtos.KeyPoints;

namespace Explorer.Tours.API.Dtos.Tours;

public class CreateTourDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Difficulty { get; set; }
    public List<string> Tags { get; set; }
    public double Price { get; set; }
    public List<TourDurationDto> Durations { get; set; }
    public List<KeyPointDto> KeyPoints { get; set; }
    public List<long> RequiredEquipmentIds { get; set; }
    public string? ThumbnailPath { get; set; }
}
