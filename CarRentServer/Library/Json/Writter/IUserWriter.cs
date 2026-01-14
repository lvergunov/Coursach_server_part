using Library.Entities;

namespace Library.Json.Writter
{
    internal interface IUserWriter : IJsonWriter<User>
    {
        public void ChangeUserPassword(long userId, string newPassword);
    }
}
