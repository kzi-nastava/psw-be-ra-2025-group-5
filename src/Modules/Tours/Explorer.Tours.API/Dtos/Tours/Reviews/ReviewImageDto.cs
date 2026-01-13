namespace Explorer.Tours.API.Dtos.Tours.Reviews
{
    public class ReviewImageDto
    {
        public long Id { get; set; }
        public string Data { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public int Order { get; set; }
    }
}