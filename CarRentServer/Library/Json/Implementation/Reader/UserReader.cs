using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Reader;
using Library.Json.Singletons;
using System.Text.Json;

namespace Library.Json.Implementation.Reader
{
    public class UserReader : IUserReader
    {
        public static UserReader Instance { get { return _instance; } }

        public UserReader() { }

        public List<User> ReadAllObjects()
        {
            string fileText = UserFileSingleton.Instance.ReadAllFile();
            UserList userList = JsonSerializer.Deserialize<UserList>(fileText) ?? throw new ReadFileException("Error in reading file with users.");
            return userList.Users;
        }

        public List<User> ReadFromTo(long startId, long finishId)
        {
            return ReadAllObjects().FindAll(x => x.Id > startId && x.Id < finishId);
        }

        public User ReadOneObject(long id)
        {
            return ReadAllObjects().Find(x => x.Id == id) ?? throw new ReadFileException($"Cannot find user with id {id}.");
        }

        public User ReadUserByEmail(string email)
        {
            return ReadAllObjects().Find(x => x.Mail.Equals(email)) ?? throw new ReadFileException($"Cannot find user with e-mail {email}.");
        }

        public User ReadUserByLogin(string login)
        {
            return ReadAllObjects().Find(x => x.Login.Equals(login)) ?? throw new ReadFileException($"Cannot find user with login {login}.");
        }

        public User ReadUserByPhone(string phone)
        {
            return ReadAllObjects().Find(x => x.Phone.Equals(phone)) ?? throw new ReadFileException($"Cannot find user with login {phone}.");
        }

        public User VerifyUser(string login, string password)
        {
            return ReadAllObjects().Find(x => x.Login.Equals(login) && x.Password.Equals(password)) ?? throw new ReadFileException("Wrong login or password");
        }

        public bool UserExists(string login, string email, string phone)
        {
            List<User> allUsers = ReadAllObjects();
            foreach (User user in allUsers) {
                if (user.Login.Equals(login) || user.Mail.Equals(email) || user.Phone.Equals(phone)) { 
                    return true;
                }
            }
            return false;
        }

        private static readonly UserReader _instance = new UserReader();
    }
}
