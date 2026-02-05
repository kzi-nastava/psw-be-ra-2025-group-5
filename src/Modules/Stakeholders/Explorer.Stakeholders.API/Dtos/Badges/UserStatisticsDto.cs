namespace Explorer.Stakeholders.API.Dtos.Badges;

public class UserStatisticsDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int Level { get; set; }
    public int AccountAgeDays { get; set; }
    public int CompletedToursCount { get; set; }
    public int CompletedChallengesCount { get; set; }
    public int PublishedToursCount { get; set; }
    public int SoldToursCount { get; set; }
    public int BlogPostsCount { get; set; }
    public int ChallengeTypesCompletedMask { get; set; }
    public bool JoinedClub { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
