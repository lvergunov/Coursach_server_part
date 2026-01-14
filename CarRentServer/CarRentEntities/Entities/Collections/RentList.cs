using System.Text.Json.Serialization;

namespace Library.Entities.Collections
{
    public class RentList
    {
        [JsonPropertyName("rents")]
        public List<Rent> Rents { get; set; } = new();
    }
}
