using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.Users
{
    public class UserPremium: Entity
    {
        public long UserId { get; set; }
        public DateTime? ValidUntil { get; set; }

        public UserPremium(long userId, DateTime? validUntil)
        {
            UserId = userId;
            ValidUntil = validUntil;
        }

        public bool IsActive()
        {
            if (ValidUntil == null) return false;
            return ValidUntil > DateTime.UtcNow;
        }

        public void Extend(DateTime newUntil)
        {
            if (ValidUntil == null || newUntil > ValidUntil)
                ValidUntil = newUntil;
        }
    }
}
