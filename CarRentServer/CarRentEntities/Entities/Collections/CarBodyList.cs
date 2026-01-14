using System.Text.Json.Serialization;

namespace Library.Entities.Collections
{
    public class CarBodyList
    {
        [JsonPropertyName("bodies")]
        public List<string> CarBodies { get; set; } = new();
    }
}
