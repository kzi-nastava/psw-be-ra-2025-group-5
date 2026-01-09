using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class CreateAuthorChallengeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int ExperiencePoints { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public long CreatedByTouristId { get; set; }
        public long KeyPointId { get; set; }
        public bool IsRequiredForSecret { get; set; }
        public bool IsRequiredForCompletion { get; set; }
    }

    //public enum ChallengeStatus
    //{
    //    Draft,
    //    Active,
    //    Archived,
    //    Pending
    //}
    //public enum ChallengeType
    //{
    //    Social,
    //    Location,
    //    Misc
    //}
}
