using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Internal;

public interface ITourSharedService
{
    TourDto Get(long id);
}