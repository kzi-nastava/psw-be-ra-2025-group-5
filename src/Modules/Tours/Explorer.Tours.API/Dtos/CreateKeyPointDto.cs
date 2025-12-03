namespace Explorer.Tours.API.Dtos;

public class CreateKeyPointDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public LocationDto Location { get; set; }
    public byte[]? Image { get; set; }
    public string? Secret { get; set; }
}
