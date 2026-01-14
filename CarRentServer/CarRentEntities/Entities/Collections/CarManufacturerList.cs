using Library.Entities;
using System.Text.Json.Serialization;

namespace CarRentEntities.Entities.Collections
{
    public class CarManufacturerList
    {
        [JsonPropertyName("manufacturers")]
        public List<string> ManufacturerMarks { get; set; } = new();
    }
}
