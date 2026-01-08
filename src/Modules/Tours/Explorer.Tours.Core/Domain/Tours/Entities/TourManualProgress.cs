using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.Tours.Entities
{
    public class TourManualProgress : Entity 
    {
        public long UserId { get; private set; }
        public string PageKey { get; private set; } 
        public bool Seen { get; private set; }

        public TourManualProgress() { }

        public TourManualProgress(long userId, string pageKey)
        {
            UserId = userId;
            PageKey = pageKey;
            Seen = false;
            Validate();
        }

        private void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid user id.");
            if (string.IsNullOrWhiteSpace(PageKey)) throw new ArgumentException("Invalid page key.");
        }

        public void MarkAsSeen() => Seen = true;
        
    }
}
