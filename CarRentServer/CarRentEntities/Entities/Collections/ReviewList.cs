using System.Text.Json.Serialization;

namespace Library.Entities.Collections
{
    public class ReviewList
    {
        [JsonPropertyName("reviews")]
        public List<CarReview> Reviews { get; set; } = new();
    }
}
