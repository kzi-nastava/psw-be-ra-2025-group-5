using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.Tours.Entities
{
    public enum TourRequestStatus { Pending, Accepted, Rejected }
    public class TourRequest : Entity
    {
        public long TouristId { get; private set; }
        public long AuthorId { get; private set; }
        public TourDifficulty Difficulty { get; private set; }
        public Location Location { get; private set; }
        public string Description { get; private set; }
        public double MaxPrice { get; private set; }
        public List<string> Tags { get; private set; }
        public TourRequestStatus Status { get; private set; }
        public long? AcceptedTourId { get; private set; }

        private TourRequest() { }

        public TourRequest(long touristId, long authorId, TourDifficulty difficulty, Location location, string description, double maxPrice, List<string> tags)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty.");

            TouristId = touristId;
            AuthorId = authorId;
            Difficulty = difficulty;
            Location = location;
            Description = description;
            MaxPrice = maxPrice;
            Tags = tags ?? new List<string>();
            Status = TourRequestStatus.Pending;
        }
    }
}
