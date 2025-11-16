using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class Monument: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public MonumentStatus Status { get; set; }
        public MonumentLocation Location { get; set; }

        public Monument(string Name, string? Description, int Year, MonumentStatus status, MonumentLocation location)
        {
            this.Name = Name;
            this.Description = Description;
            this.Year = Year;
            this.Status = status;
            this.Location = location;
        }
    }
}
