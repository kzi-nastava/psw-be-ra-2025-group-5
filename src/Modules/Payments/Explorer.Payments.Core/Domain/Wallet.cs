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
        public long UserId { get; private set; }
        public double Balance { get; private set; } = 0;

        private Wallet() { }
        public Wallet(long userId)
        {
            UserId = userId;
            Balance = 0;
            Validate();
        }

        private void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid UserId");
        }
        public void Credit(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            Balance += amount;
        }

        public void Debit(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient balance");

            Balance -= amount;
        }


    }
}
