using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos.Gifts
{
    public class GiftDto
    {
        public long Id { get; set; }
        public long DonorId { get; set; }
        public long RecipientId { get; set; }
        public long TourId { get; set; }
        public double Price { get; set; }
        public string? Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
