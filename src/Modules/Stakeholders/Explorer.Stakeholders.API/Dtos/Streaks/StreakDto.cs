namespace Explorer.Stakeholders.API.Dtos.Streaks
{
    public class StreakDto
    {
        public long UserId { get; set; }
        public DateOnly StartDate { get; set; }
        public int LongestStreak { get; set; }
        public int CurrentStreak { get; set; }
        public DateOnly LastActivityDate { get; set; }
    }
}
