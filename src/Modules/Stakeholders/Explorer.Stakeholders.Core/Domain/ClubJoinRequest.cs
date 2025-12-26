using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubJoinRequest
    {
        public long Id { get; set; }
        public long ClubId { get; set; }
        public long TouristId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long NotificationId { get; set; }

        public ClubJoinRequest(long clubId, long touristId, long notificationId)
        {
            ClubId = clubId;
            TouristId = touristId;
            NotificationId = notificationId;
            CreatedAt = DateTime.UtcNow;
        }
    }

}
