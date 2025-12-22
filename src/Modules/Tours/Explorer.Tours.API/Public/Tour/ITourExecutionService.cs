using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Locations;
using Explorer.Tours.API.Dtos.Tours.Executions;

namespace Explorer.Tours.API.Public.Tour
{
    public interface ITourExecutionService
    {
        StartExecutionResultDto StartExecution(long userId, long tourId);
        CheckProximityDto CheckProximity(long executionId, LocationDto location);
        void CompleteExecution(long executionId);
        void AbandonExecution(long executionId);
        TourExecutionDto GetExecution(long executionId);
        List<TourExecutionDto> GetExecutionsForUser(long userId);
        void ExpireOldTours();
    }
}
