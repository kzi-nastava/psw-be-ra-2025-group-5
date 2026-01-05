using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Percentage { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long AuthorId { get; set; }
        public long? TourId { get; set; }

    }
}
