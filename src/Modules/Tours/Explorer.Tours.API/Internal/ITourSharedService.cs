using Explorer.Tours.API.Dtos.Tours;

namespace Explorer.Tours.API.Internal;

public interface ITourSharedService
{
    TourDto Get(long id);
}