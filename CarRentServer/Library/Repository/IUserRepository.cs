using Library.Entities;

namespace Library.Repository
{
    public interface IUserRepository : ICommonRepository<User>
    {
        public void ChangeUserPassword(long userId, string newPassword);
        
        public User ReadUserByLogin(string login);

        public User VerifyUser(string login, string password);

        public User ReadUserByEmail(string email);

        public User ReadUserByPhone(string phone);

        public bool UserExists(string login, string email, string phone);
    }
}
