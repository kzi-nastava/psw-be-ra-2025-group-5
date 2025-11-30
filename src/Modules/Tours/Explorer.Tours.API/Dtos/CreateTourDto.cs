namespace Explorer.Tours.API.Dtos;

public class CreateTourDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Difficulty { get; set; }
    public List<string> Tags { get; set; }
    public double Price { get; set; }
}
