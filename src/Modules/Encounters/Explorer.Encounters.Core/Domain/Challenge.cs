using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain;

public class Challenge : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int ExperiencePoints { get; private set; }
    public ChallengeStatus Status { get; private set; }
    public ChallengeType Type { get; private set; }
    public long? CreatedByTouristId { get; private set; }
    public int? RequiredParticipants { get; private set; }
    public int? RadiusInMeters { get; private set; }
    public string? ImageUrl { get; private set; }

    public Challenge() { }

    public Challenge(string name, string description, double latitude, double longitude, int experiencePoints, ChallengeStatus status, ChallengeType type,
        long? createdByTouristId = null, int? requiredParticipants = null, int? radiusInMeters = null, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
        if (experiencePoints < 0) throw new ArgumentException("Experience points must be non-negative.");
        if (latitude < -90 || latitude > 90) throw new ArgumentException("Latitude must be between -90 and 90.");
        if (longitude < -180 || longitude > 180) throw new ArgumentException("Longitude must be between -180 and 180.");
        if (type == ChallengeType.Social)
        {
            if (requiredParticipants is null || requiredParticipants < 2)
                throw new ArgumentException("Social challenge must require at least 2 participants.");

            if (radiusInMeters is null || radiusInMeters <= 0)
                throw new ArgumentException("Social challenge must have a positive radius.");
        }

        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        ExperiencePoints = experiencePoints;
        Status = status;
        Type = type;
        CreatedByTouristId = createdByTouristId;
        RequiredParticipants = requiredParticipants;
        RadiusInMeters = radiusInMeters;
    }

    public void Update(string name, string description, double latitude, double longitude, int experiencePoints, ChallengeStatus status, ChallengeType type,
        int? requiredParticipants = null, int? radiusInMeters = null, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
        if (experiencePoints < 0) throw new ArgumentException("Experience points must be non-negative.");
        if (latitude < -90 || latitude > 90) throw new ArgumentException("Latitude must be between -90 and 90.");
        if (longitude < -180 || longitude > 180) throw new ArgumentException("Longitude must be between -180 and 180.");
        if (type == ChallengeType.Social)
        {
            if (requiredParticipants is null || requiredParticipants < 2)
                throw new ArgumentException("Social challenge must require at least 2 participants.");

            if (radiusInMeters is null || radiusInMeters <= 0)
                throw new ArgumentException("Social challenge must have a positive radius.");
        }

        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        ExperiencePoints = experiencePoints;
        Status = status;
        Type = type;
        RequiredParticipants = requiredParticipants;
        RadiusInMeters = radiusInMeters;
        ImageUrl = imageUrl;
    }

    public void UpdateImage(string? imagePath)
    {
        ImageUrl = imagePath;
    }
}
public enum ChallengeStatus
{
    Draft,
    Active,
    Archived,
    Pending
}
public enum ChallengeType
{
    Social,
    Location,
    Misc
}