using System.Text.Json.Serialization;

namespace Library.Entities
{
    public class FilterJson
    {
        [JsonPropertyName("start-date")]
        public DateTime? StartDate { get; }

        [JsonPropertyName("end-date")]
        public DateTime? EndDate { get; }

        [JsonPropertyName("low-cost")]
        public float? LowCost { get; }
        
        [JsonPropertyName("high-cost")]
        public float? HighCost { get; }
        
        [JsonPropertyName("car-body")]
        public string? CarBody { get; }

        [JsonPropertyName("manufacturer")]
        public string? Manufacturer { get; }

        [JsonPropertyName("name")]
        public string? Name { get; }

        [JsonPropertyName("active")]
        public bool? Active { get; }

        public FilterJson(DateTime? startDate, DateTime? endDate, float? lowCost, float? highCost, string? carBody, string? manufacturer, 
            string? name, bool? active) { 
            StartDate = startDate;
            EndDate = endDate;
            LowCost = lowCost;
            HighCost = highCost;
            CarBody = carBody;
            Manufacturer = manufacturer;
            Name = name;
            Active = active;
        }
    }
}
