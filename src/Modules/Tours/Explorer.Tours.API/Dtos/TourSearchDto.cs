namespace Explorer.Tours.API.Dtos;

public class TourSearchDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Distance { get; set; }
    public string? Difficulty { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public List<string>? Tags { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
}

