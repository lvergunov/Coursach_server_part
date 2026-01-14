using System.Text.Json.Serialization;

namespace Library.Entities.Collections
{
    public class UserList
    {
        [JsonPropertyName("users")]
        public List<User> Users { get; set; } = new List<User>();
    }
}
