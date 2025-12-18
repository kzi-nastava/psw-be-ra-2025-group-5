namespace Explorer.Tours.API.Internal.Statistics;

    public class TourStatisticsItemDto
    {
        public string Difficulty { get; set; } = string.Empty;
        public IReadOnlyCollection<string> Tags { get; set; } = Array.Empty<string>();
    }

