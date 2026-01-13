using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos.Monuments
{
    public class MonumentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MonumentStatus Status { get; set; }
        public MonumentLocationDto Location { get; set; }
    }
    public enum MonumentStatus
    {
        Active, Inactive
    }
}
