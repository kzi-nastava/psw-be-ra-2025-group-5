using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos.Wallet
{
    public class WalletDto
    {
        public long TouristId { get; set; }
        public double Balance { get; set; }
    }
}
