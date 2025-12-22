using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Shared;

namespace Explorer.Stakeholders.Core.Domain.Positions;

public class Position : Entity
{
    public long TouristId { get; private set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Position(long touristId, double latitude, double longitude)
    {
        Guard.AgainstNull(touristId, nameof(touristId));
        Guard.AgainstOutOfRange(latitude, nameof(latitude), -90, 90);
        Guard.AgainstOutOfRange(longitude, nameof(longitude), -180, 180);

        TouristId = touristId;
        Latitude = latitude;
        Longitude = longitude;
    }
}
