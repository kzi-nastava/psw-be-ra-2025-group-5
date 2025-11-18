namespace Explorer.Stakeholders.API.Dtos
{
    public class AppRatingDto
    {
        public long UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
