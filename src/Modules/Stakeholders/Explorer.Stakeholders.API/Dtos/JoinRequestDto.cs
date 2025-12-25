using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class JoinRequestDto
    {
        public long Id { get; set; }
        public long ClubId { get; set; }
        public string TouristName { get; set; }
        public string TouristSurname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
