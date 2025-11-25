namespace Explorer.Stakeholders.API.Dtos;

public class PositionDto
{
    public long Id { get; set; }
    public long PersonId { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}