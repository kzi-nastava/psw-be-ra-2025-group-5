namespace Explorer.Tours.API.Dtos;

public class KeyPointDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public LocationDto Location { get; set; }
    public byte[]? Image { get; set; }
    public string? Secret { get; set; }
    public int Position { get; set; }
}
