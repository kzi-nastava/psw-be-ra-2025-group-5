using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class Gift : AggregateRoot
    {
        public long DonorId { get; private set; }
        public long RecipientId { get; private set; }
        public long TourId { get; private set; }
        public double Price { get; private set; }
        public string? Message { get; private set; }
        public string Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Gift() { }

        public Gift(long donorId, long recipientId, long tourId, double price, string? message)
        {
            DonorId = donorId;
            RecipientId = recipientId;
            TourId = tourId;
            Price = price;
            Message = message;
            Status = "Completed";
            CreatedAt = DateTime.UtcNow;
        }
    }
}
