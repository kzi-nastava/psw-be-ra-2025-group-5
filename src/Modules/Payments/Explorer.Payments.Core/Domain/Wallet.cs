using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class Wallet
    {
        public long Balance { get; private set; } = 0;
        public long TouristId { get; private set; }

        public Wallet(long userId)
        {
            TouristId = userId;
            Balance = 0;
            Validate();
        }

        private void Validate()
        {
            if (TouristId == 0) throw new ArgumentException("Invalid UserId");
        }

    }
}
