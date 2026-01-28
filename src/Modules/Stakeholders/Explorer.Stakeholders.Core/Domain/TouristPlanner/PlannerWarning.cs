using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Explorer.Stakeholders.Core.Domain.TouristPlanner;

public enum PlannerWarningType
{
    DurationTooShort,
    NoBreaks,
    TooManyTours,
    LateNightActivity
} 
public class PlannerWarning
{
     public PlannerWarningType Type { get; set; }
     public string Message { get; set; }
     public List<long> AffectedBlockIds { get; set; } = new();
}

