using Explorer.Tours.API.Dtos.Locations;

namespace Explorer.Tours.API.Dtos.KeyPoints;

public class CreateKeyPointDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public LocationDto Location { get; set; }
    public byte[]? Image { get; set; }
    public string? Secret { get; set; }
    public double TourLength { get; set; }
}
