using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos.Gifts
{
    public class CreateGiftDto
    {
        public long RecipientId { get; set; }
        public long TourId { get; set; }
        public string? Message { get; set; }
    }
}
