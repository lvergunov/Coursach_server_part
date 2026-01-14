using System.Text.Json.Serialization;

namespace Library.Entities
{
    public class CarReview : CommonEntity
    {
        [JsonPropertyName("car-id")]
        public long CarId { get; }

        [JsonPropertyName("user-id")]
        public long UserId { get; }

        [JsonPropertyName("rate")]
        public ushort Rate { get; }

        [JsonPropertyName("text")]
        public string Text { get; }

        public CarReview(long id, long carId, long userId, ushort rate, string text) : base(id)
        {
            CarId = carId;
            UserId = userId;
            Rate = rate;
            Text = text;
        }
    }
}
