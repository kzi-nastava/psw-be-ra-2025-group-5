using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ovde ne može da pronađe Core, već samo API
// using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.API.Dtos.Users
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "Administrator", "Author"

        // Ovo iz nekog razloga ne radi?
        // public UserRole Role { get; set; } 

    }
}
