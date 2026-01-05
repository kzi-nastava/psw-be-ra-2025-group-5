using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubInvite
    {
        public long Id { get; set; }
        public long ClubId { get; set; }
        public long TouristId { get; set; }
        public DateTime CreatedAt { get; set; }

        public long NotificationId { get; set; }

        public ClubInvite(long clubId, long touristId, DateTime createdAt, long notificationId)
        {
            ClubId = clubId;
            TouristId = touristId;
            CreatedAt = createdAt;
            NotificationId = notificationId;    
        }

    }
}
