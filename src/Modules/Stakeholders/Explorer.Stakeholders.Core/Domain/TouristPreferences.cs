using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class TouristPreferences : Entity
{
    public long UserId { get; private set; }
    public TourDifficulty PreferredDifficulty { get; private set; }
    public Dictionary<TransportationType, int> TransportationRatings { get; private set; }
    public List<string> PreferredTags { get; private set; }

    public TouristPreferences() { }

    public TouristPreferences(long userId, TourDifficulty preferredDifficulty, Dictionary<TransportationType, int> transportationRatings, List<string> preferredTags)
    {
        UserId = userId;
        PreferredDifficulty = preferredDifficulty;
        TransportationRatings = transportationRatings ?? new Dictionary<TransportationType, int>();
        PreferredTags = preferredTags ?? new List<string>();
        Validate();
    }

    public void Update(TourDifficulty preferredDifficulty, Dictionary<TransportationType, int> transportationRatings, List<string> preferredTags)
    {
        PreferredDifficulty = preferredDifficulty;
        TransportationRatings = transportationRatings ?? new Dictionary<TransportationType, int>();
        PreferredTags = preferredTags ?? new List<string>();
        Validate();
    }

    private void Validate()
    {
        if (UserId <= 0) throw new ArgumentException("Invalid UserId");
        foreach (var rating in TransportationRatings.Values)
        {
            if (rating < 0 || rating > 3) throw new ArgumentException("Transportation rating must be between 0 and 3");
        }
        if (PreferredTags == null) throw new ArgumentException("PreferredTags cannot be null");
    }
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