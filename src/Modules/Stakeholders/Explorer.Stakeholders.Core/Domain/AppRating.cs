using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class AppRating : Entity
{
    public long UserId { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedAt { get;  set; }
    public DateTime UpdatedAt { get; set; }

    protected AppRating() { }
    public AppRating(long userId, int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be 1–5.");

    

        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be 1–5.");

        Rating = rating;
        Comment = comment;
        UpdatedAt = DateTime.UtcNow;
    }
}
