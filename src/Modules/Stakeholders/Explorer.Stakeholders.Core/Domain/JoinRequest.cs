using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class JoinRequest
    {
        enum RequestStatus
        {
            PENDING,
            ACCEPTED,
            REJECTED
        }
        public long Id { get; set; }
        public long TouristId { get; set; }
        public long ClubId { get; set; }
        public DateTime CreatedAt { get; set; }
        RequestStatus Status { get; set; }

        public JoinRequest(long touristId, long clubId, DateTime createAt)
        {
            TouristId = touristId;
            ClubId = clubId;
            CreatedAt = createAt;
            Status = RequestStatus.PENDING;
        }
    }
}
