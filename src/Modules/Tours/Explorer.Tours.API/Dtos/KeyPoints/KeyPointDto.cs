using Explorer.Tours.API.Dtos.Locations;

namespace Explorer.Tours.API.Dtos.KeyPoints;

public class KeyPointDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public LocationDto Location { get; set; }
    public string? ImagePath { get; set; }
    public string? Secret { get; set; }
    public int Position { get; set; }
}
