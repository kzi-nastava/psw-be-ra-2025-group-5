namespace Explorer.Stakeholders.API.Dtos;

public class TouristPreferencesDto
{
    public long UserId { get; set; }
    public TourDifficulty PreferredDifficulty { get; set; }
    public Dictionary<TransportationType, int> TransportationRatings { get; set; }
    public List<string> PreferredTags { get; set; }
}

public enum TourDifficulty
{
    Easy,
    Medium,
    Hard
}

public enum TransportationType
{
    Walking,
    Bicycle,
    Car,
    Boat
}