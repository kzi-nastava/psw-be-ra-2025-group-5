using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain;

public class Challenge : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int ExperiencePoints { get; private set; }
    public ChallengeStatus Status { get; private set; }
    public ChallengeType Type { get; private set; }

    public Challenge(string name, string description, double latitude, double longitude, int experiencePoints, ChallengeStatus status, ChallengeType type)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
        if (experiencePoints < 0) throw new ArgumentException("Experience points must be non-negative.");
        if (latitude < -90 || latitude > 90) throw new ArgumentException("Latitude must be between -90 and 90.");
        if (longitude < -180 || longitude > 180) throw new ArgumentException("Longitude must be between -180 and 180.");

        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        ExperiencePoints = experiencePoints;
        Status = status;
        Type = type;
    }

    public void Update(string name, string description, double latitude, double longitude, int experiencePoints, ChallengeStatus status, ChallengeType type)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
        if (experiencePoints < 0) throw new ArgumentException("Experience points must be non-negative.");
        if (latitude < -90 || latitude > 90) throw new ArgumentException("Latitude must be between -90 and 90.");
        if (longitude < -180 || longitude > 180) throw new ArgumentException("Longitude must be between -180 and 180.");

        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        ExperiencePoints = experiencePoints;
        Status = status;
        Type = type;
    }
}
public enum ChallengeStatus
{
    Draft,
    Active,
    Archived
}
public enum ChallengeType
{
    Social,
    Location,
    Misc
}