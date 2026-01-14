using System.Text.Json.Serialization;

namespace Library.Entities
{
    public class User : CommonEntity
    {

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("login")]
        public string Login { get; }

        [JsonPropertyName("phone")]
        public string Phone { get; }

        [JsonPropertyName("mail")]
        public string Mail { get; }

        [JsonPropertyName("password")]
        public string Password { get; }

        [JsonPropertyName("is-admin")]
        public bool IsAdmin { get; }

        public User(long id, string name, string login,
            string phone, string mail, string password, bool isAdmin = false) : base(id)
        {
            Name = name;
            Login = login;
            Phone = phone;
            Mail = mail;
            Password = password;
            IsAdmin = isAdmin;
        }
    }
}
