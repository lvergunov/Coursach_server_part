using Library.Entities;
using Library.Exception;
using Library.Json.Implementation.Reader;
using Library.Json.Implementation.Writter;

namespace Library.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        public void ChangeUserPassword(long userId, string newPassword)
        {
            UserWriter.Instance.ChangeUserPassword(userId, newPassword);
        }

        public void ClearAll()
        {
            UserWriter.Instance.ResetData();
        }

        public List<User> FindAll()
        {
            return UserReader.Instance.ReadAllObjects();
        }

        public User FindById(long id)
        {
            return UserReader.Instance.ReadOneObject(id);
        }

        public List<User> FindFromTo(long leftId, long rightId)
        {
            return UserReader.Instance.ReadFromTo(leftId, rightId);
        }

        public User ReadUserByEmail(string email)
        {
            return UserReader.Instance.ReadUserByEmail(email);
        }

        public User ReadUserByLogin(string login)
        {
            return UserReader.Instance.ReadUserByLogin(login);
        }

        public User ReadUserByPhone(string phone)
        {
            return UserReader.Instance.ReadUserByPhone(phone);
        }

        public void RemoveById(long id)
        {
            UserWriter.Instance.Delete(id);
        }

        public void Save(User entity)
        {
            List<User> allUsers = UserReader.Instance.ReadAllObjects();
            if (allUsers.Any(u => u.Mail.Equals(entity.Mail))) {
                throw new UserExistsException("This email is already binded");
            }
            if (allUsers.Any(u => u.Phone.Equals(entity.Phone))) {
                throw new UserExistsException("This phone is already binded");
            }
            if (allUsers.Any(u => u.Login.Equals(entity.Login))) {
                throw new UserExistsException("User with this login already exists");
            }
            if (allUsers.Any(u => u.Id == entity.Id))
            {
                throw new UserExistsException("User with this id already exists");
            }
            long biggestId = 1;
            if (allUsers.Count != 0)
                biggestId = allUsers.Max(c => c.Id) + 1;
            UserWriter.Instance.Write(new User(biggestId, entity.Name, entity.Login, entity.Phone, entity.Mail, entity.Password, entity.IsAdmin));
        }

        public bool UserExists(string login, string email, string phone)
        {
            return UserReader.Instance.UserExists(login, email, phone);
        }

        public User VerifyUser(string login, string password)
        {
            return UserReader.Instance.VerifyUser(login, password);
        }
    }
}
