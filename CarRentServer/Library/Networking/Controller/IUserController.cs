namespace Library.Networking.Controller
{
    public interface IUserController : ICommonController
    {
        public string GetUserByMail(string mail, out QueryResultFlag queryResult);
        public string GetUserByLogin(string login, out QueryResultFlag queryResult);
        public string GetUserByPhone(string phone, out QueryResultFlag queryResult);
        public string VerifyUser(string login, string password, out QueryResultFlag queryResult);
        public string ChangeUsersPassword(long userId, string password, out QueryResultFlag queryResult);
    }
}
