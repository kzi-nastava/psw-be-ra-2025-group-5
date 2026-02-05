using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos.TouristPlanner
{
    public class PlannerGenerationOptionsDto
    {
        public DateOnly StartDate { get; set; }
        public int NumberOfDays { get; set; }
        public int MaxToursPerDay { get; set; }
    }
}
