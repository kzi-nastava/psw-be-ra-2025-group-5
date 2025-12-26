namespace Explorer.Stakeholders.API.Dtos.AppRatings
{
    public class AppRatingDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Username { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
