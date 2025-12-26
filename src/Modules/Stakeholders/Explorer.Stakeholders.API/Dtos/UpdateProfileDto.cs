using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class UpdateProfileDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? Biography { get; set; }
        public string? Motto { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
