using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public.TouristPlanner
{
    public interface IPlannerGenerationService
    {
        PlannerDto GeneratePlan(long touristId);
    }
}
