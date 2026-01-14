using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain.Tours.Entities;

public class TourSearchHistory : Entity
{
    public long UserId { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double Distance { get; private set; }
    public string? Difficulty { get; private set; }
    public double? MinPrice { get; private set; }
    public double? MaxPrice { get; private set; }
    public List<string> Tags { get; private set; }
    public string? SortBy { get; private set; }
    public string? SortOrder { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private TourSearchHistory()
    {
        Tags = new List<string>();
        CreatedAt = DateTime.UtcNow;
    }

    public TourSearchHistory(long userId, double latitude, double longitude, double distance, string? difficulty, double? minPrice, double? maxPrice, List<string>? tags, string? sortBy, string? sortOrder)
    {
        UserId = userId;
        Latitude = latitude;
        Longitude = longitude;
        Distance = distance;
        Difficulty = difficulty;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        Tags = tags ?? new List<string>();
        SortBy = sortBy;
        SortOrder = sortOrder;
        CreatedAt = DateTime.UtcNow;
    }
}

