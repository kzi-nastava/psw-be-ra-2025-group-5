using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos.Tours.PurchaseTokens;

public class CreateTourPurchaseTokenDto
{
    public long TourId { get; set; }
    public long TouristId { get; set; }
}
