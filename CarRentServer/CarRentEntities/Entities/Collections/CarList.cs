using System.Text.Json.Serialization;

namespace Library.Entities.Collections
{
    public class CarList
    {
        [JsonPropertyName("cars")]
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
