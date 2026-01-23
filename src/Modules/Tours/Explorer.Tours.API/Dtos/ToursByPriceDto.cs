using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class ToursByPriceDto
    {
        public int Count { get; set; }
        public string PriceRange { get; set; }
    }
}