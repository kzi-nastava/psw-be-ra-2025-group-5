using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public.TouristPlanner
{
    public interface IPlannerValidationService
    {
        List<PlannerWarningDto> ValidateDay(PlannerDayDto dayDto, Dictionary<long, int> systemDurations);
    }
}
