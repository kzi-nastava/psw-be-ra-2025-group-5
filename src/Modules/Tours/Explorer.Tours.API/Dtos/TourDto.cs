namespace Explorer.Tours.API.Dtos;

public class TourDto
{
    public long Id { get; set; }
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Difficulty { get; set; }
    public List<string> Tags { get; set; }
    public double Price { get; set; }
    public string Status { get; set; }
    public DateTime? PublishedDate { get; set; }
    public DateTime? ArchivedDate { get; set; }
    public List<KeyPointDto> KeyPoints { get; set; }
    public List<TourReviewDto> Reviews { get; set; }
    public List<TourDurationDto> Durations { get; set; }
    public double TourLength { get; set; }
}