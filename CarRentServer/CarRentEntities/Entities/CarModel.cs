using System.Text.Json.Serialization;

namespace Library.Entities
{
    public class CarModel
    {
        public CarModel(string body, string mark, string name)
        {
            Body = body;
            Mark = mark;
            Name = name;
        }

        [JsonPropertyName("body")]
        public string Body { get; }

        [JsonPropertyName("mark")]
        public string Mark { get; }

        [JsonPropertyName("name")]
        public string Name { get; }
    }
}
