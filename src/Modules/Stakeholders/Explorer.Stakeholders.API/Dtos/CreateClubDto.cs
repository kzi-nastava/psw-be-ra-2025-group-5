using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class CreateClubDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
