using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos.Wallet
{
    public class PaymentDto
    {
        public long TouristId { get; set; }
        public long TourId { get; set; }
        public decimal Price { get; set; }
        public DateTime PaidAt { get; set; }
        public string? Status { get; set; }
    }   
}
