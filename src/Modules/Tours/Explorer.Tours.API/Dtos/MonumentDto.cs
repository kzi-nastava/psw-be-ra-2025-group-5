using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class MonumentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public MonumentStatus Status { get; set; }
        public MonumentLocationDto Location { get; set; }
    }
    public enum MonumentStatus
    {
        Active, Inactive
    }
}
