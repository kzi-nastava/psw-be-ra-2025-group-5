using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class KeyPointChallengeDto
    {
        public long KeyPointId { get; set; }
        public long ChallengeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ExperiencePoints { get; private set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public bool IsRequiredForSecret { get; set; }
        public bool IsRequiredForCompletion { get; set; }
    }
}
