using Explorer.Tours.API.Dtos.KeyPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos.Tours.Executions
{
    public class StartExecutionResultDto
    {
        public long ExecutionId { get; set; }
        public KeyPointDto? NextKeyPoint { get; set; } 
        public DateTime StartTime { get; set; }
    }
}
