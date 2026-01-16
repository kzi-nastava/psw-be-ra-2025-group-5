using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ClubJoinRequestDto
    {
        public long Id { get; set; }
        public long ClubId { get; set; }
        public string TouristUsername { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TouristName { get; set; }
        public long TouristId { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
