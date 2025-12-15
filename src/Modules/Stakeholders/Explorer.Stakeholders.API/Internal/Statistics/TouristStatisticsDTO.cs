namespace Explorer.Stakeholders.API.Internal.Statistics
{
    public class TouristStatisticsDto
    {
        public int PurchasedToursCount { get; set; }
        public int CompletedToursCount { get; set; }
        public string? MostCommonTag { get; set; }
        public string? MostCommonDifficulty { get; set; }
    }
}
