using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Monuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;



namespace Explorer.Tours.Core.Domain.Monuments
{
    public class Monument: Entity
    {
        public enum MonumentStatus
        {
            Active, Inactive
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public MonumentStatus Status { get; set; }
        public MonumentLocation Location { get; set; }

        public Monument() { }
        public Monument(string Name, string? Description, int Year, MonumentStatus status, MonumentLocation location)
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name.");
            this.Name = Name;
            this.Description = Description;
            this.Year = Year;
            Status = status;
            Location = location;
        }
    }
}
