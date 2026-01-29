
using System.Text.Json.Serialization;

namespace Explorer.Stakeholders.API.Dtos.TouristPlanner;

public class CreatePlannerTimeBlockDto
{
    public long TourId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Duration { get; set; }
    public string TransportType { get; set; }
}



