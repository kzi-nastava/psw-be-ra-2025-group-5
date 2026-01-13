namespace Explorer.Stakeholders.API.Dtos
{
    public class TouristStatisticsDto
    {
        public int PurchasedToursCount { get; set; }
        public int CompletedToursCount { get; set; }
        public string? MostCommonTag { get; set; }
        public string? MostCommonDifficulty { get; set; }
    }
}
