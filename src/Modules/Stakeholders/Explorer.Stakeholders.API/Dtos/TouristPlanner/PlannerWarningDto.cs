using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos.TouristPlanner
{
    public class PlannerWarningDto
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public List<long> AffectedBlockIds { get; set; }
    }
}
