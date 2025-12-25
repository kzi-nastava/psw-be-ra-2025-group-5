using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Club : Entity
    {
        public enum ClubStatus
        {
            Active,
            Closed
        }

        public string Name { get;  set; }
        public string Description { get;  set; }
        public List<string> ImagePaths { get; private set; }
        public long CreatorId { get;  set; }
        public ClubStatus Status { get; set; } = ClubStatus.Active;
        public List<ClubMember> Members { get; set; } = new();
        private Club() { }
        public Club(string name, string description, List<string>? imagePaths, long creatorId, ClubStatus clubStatus)
        {
            Name = name;
            Description = description;
            ImagePaths = imagePaths ?? new List<string>();
            CreatorId = creatorId;
            Status = clubStatus;
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Club name cannot be empty.");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Club description cannot be empty.");
            //if (CreatorId < 0) throw new ArgumentException("Invalid creator ID.");
        }
        public bool IsActive() => Status == ClubStatus.Active;
        public void CloseClub() => Status = ClubStatus.Closed;
        public void OpenClub() => Status = ClubStatus.Active;
        public bool IsMember(long touristId)
        {
            return Members.Any(m => m.TouristId == touristId);
        }

        public void AddMember(long touristId)
        {
            if (!IsMember(touristId))
            {
                Members.Add(new ClubMember
                {
                    ClubId = this.Id,
                    TouristId = touristId,
                    JoinedAt = DateTime.UtcNow
                });
            }
        }
        public void RemoveMember(long touristId)
        {
            var member = Members.FirstOrDefault(m => m.TouristId == touristId);
            if (member != null)
                Members.Remove(member);
        }

    }
}
