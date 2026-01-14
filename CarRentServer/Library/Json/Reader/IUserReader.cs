using Library.Entities;

namespace Library.Json.Reader
{
    internal interface IUserReader : IJsonReader<User>
    {
        public User ReadUserByLogin(string login);

        public User VerifyUser(string login, string password);

        public User ReadUserByEmail(string email);

        public User ReadUserByPhone(string phone);

        public bool UserExists(string login, string email, string phone);
    }
}
