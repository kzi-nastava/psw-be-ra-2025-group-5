using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class WalletUserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string? ProfileImagePath { get; set; }
        public double Balance { get; set; }
    }
}
