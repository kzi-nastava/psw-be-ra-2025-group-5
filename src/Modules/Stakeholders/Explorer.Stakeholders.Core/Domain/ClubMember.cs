using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubMember
    {
        public long ClubId { get; set; }
        public long TouristId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
