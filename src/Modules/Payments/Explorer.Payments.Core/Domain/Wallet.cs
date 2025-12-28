using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class Wallet : Entity
    {
        public long TouristId { get; private set; }
        public double Balance { get; private set; } = 0;

        private Wallet() { }
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
        public void Credit(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            Balance += amount;
        }
    }
}
