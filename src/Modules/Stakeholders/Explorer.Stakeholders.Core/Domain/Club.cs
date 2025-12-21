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
        public string Name { get;  set; }
        public string Description { get;  set; }
        public List<string> ImagePaths { get; private set; }
        public long CreatorId { get;  set; }
        public Club(string name, string description, List<string>? imagePaths, long creatorId)
        {
            Name = name;
            Description = description;
            ImagePaths = imagePaths ?? new List<string>();
            CreatorId = creatorId;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Club name cannot be empty.");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Club description cannot be empty.");
            if (ImagePaths.Count == 0) throw new ArgumentException("Club must have at least one image.");
            //if (CreatorId < 0) throw new ArgumentException("Invalid creator ID.");
        }
    }
}
