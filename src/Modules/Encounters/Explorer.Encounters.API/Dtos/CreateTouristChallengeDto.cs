using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class CreateTouristChallengeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int ExperiencePoints { get; set; }
        public int? RequiredParticipants { get; set; }
        public int? RadiusInMeters { get; set; }
        public string Type { get; set; }
        public IFormFile? Image { get; set; }
    }
}
