using System.Text.Json.Serialization;
using Explorer.Tours.API.Mappers;

namespace Explorer.Tours.API.Dtos;

public class UpdateTourDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Difficulty { get; set; }
    public List<string> Tags { get; set; }
    public double Price { get; set; }
    public byte[]? ThumbnailImage { get; set; }
    public string? ThumbnailContentType { get; set; }
    [JsonConverter(typeof(TourDurationDtoListConverter))]
    public List<TourDurationDto>? Durations { get; set; }
}
