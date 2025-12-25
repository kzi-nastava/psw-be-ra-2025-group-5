using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class ToursByPrice
    {
        public int Count { get; }
        public string PriceRange { get; }

        public ToursByPrice()
        {

        }
        public ToursByPrice(int count, string priceRange)
        {
            Count = count;
            PriceRange = priceRange;
        }
    }
}
