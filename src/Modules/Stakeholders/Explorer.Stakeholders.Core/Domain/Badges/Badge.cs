using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Badges;

public enum BadgeRank
{
    Bronze = 0,
    Silver = 1,
    Gold = 2,
    Epic = 3
}

public enum BadgeType
{
    Level = 0,
    AccountAge = 1,
    CompletedTours = 2,
    CompletedChallenges = 3,
    PublishedTours = 4,
    SoldTours = 5,
    AllChallengeTypesCompleted = 6,
    BlogPosts = 7,
    ClubMember = 8
}

public enum BadgeRole
{
    Both = 0,      // Za oba (Author i Tourist)
    Tourist = 1,   // Samo za turiste
    Author = 2     // Samo za autore
}

public class Badge : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImagePath { get; private set; }
    public BadgeRank Rank { get; private set; }
    public BadgeType Type { get; private set; }
    public int RequiredValue { get; private set; }
    public BadgeRole Role { get; private set; }

    private Badge() { }

    public Badge(string name, string description, string imagePath, BadgeRank rank, BadgeType type, int requiredValue, BadgeRole role = BadgeRole.Both)
    {
        Name = name;
        Description = description;
        ImagePath = imagePath;
        Rank = rank;
        Type = type;
        RequiredValue = requiredValue;
        Role = role;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Badge Name");
        if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Badge Description");
        if (string.IsNullOrWhiteSpace(ImagePath)) throw new ArgumentException("Invalid Badge ImagePath");
        if (RequiredValue < 0) throw new ArgumentException("RequiredValue cannot be negative");
    }
}

