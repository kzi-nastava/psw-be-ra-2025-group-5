using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Shared;

namespace Explorer.Stakeholders.Core.Domain;

public class TouristLocation : Entity
{
    public long PersonId { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public TouristLocation(long personId, double latitude, double longitude)
    {
        Guard.AgainstNull(personId, nameof(personId));
        Guard.AgainstOutOfRange(latitude, nameof(latitude), -90, 90);
        Guard.AgainstOutOfRange(longitude, nameof(longitude), -180, 180);

        PersonId = personId;
        Latitude = latitude;
        Longitude = longitude;
    }
}
