using Microsoft.AspNetCore.Http;

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
        public DateTime? EndChallenge { get; set; }
        public int? DailyParticipantLimit { get; set; }
        public IFormFile? Image { get; set; }
    }
}
