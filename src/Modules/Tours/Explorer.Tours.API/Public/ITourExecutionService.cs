using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public
{
    public interface ITourExecutionService
    {
        StartExecutionResultDto StartExecution(long userId, long tourId);
        CheckProximityDto CheckProximity(long executionId, LocationDto location);
        void CompleteExecution(long executionId);
        void AbandonExecution(long executionId);
        TourExecutionDto GetExecution(long executionId); 
    }
}
