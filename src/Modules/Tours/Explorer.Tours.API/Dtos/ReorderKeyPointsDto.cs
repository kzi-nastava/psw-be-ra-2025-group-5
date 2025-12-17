namespace Explorer.Tours.API.Dtos;

public class ReorderKeyPointsDto
{
    public List<long> OrderedKeyPointIds { get; set; }
    public double TourLength { get; set; }
}
