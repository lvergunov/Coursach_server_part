using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Implementation.Reader;
using Library.Json.Singletons;
using Library.Json.Writter;
using System.Text.Json;

namespace Library.Json.Implementation.Writter
{
    public class UserWriter : IUserWriter
    {
        public static UserWriter Instance { get { return _instance; } }

        public void ChangeUserPassword(long userId, string newPassword)
        {
            User searched = UserReader.Instance.ReadOneObject(userId);
            Update(userId, new User(userId, searched.Name, searched.Login, searched.Phone, searched.Mail, newPassword));
        }

        public void ResetData()
        {
            UserFileSingleton.Instance.TearDownAll();
        }

        public void Update(long id, User entity)
        {
            string readText = UserFileSingleton.Instance.ReadAllFile();
            UserList userList = JsonSerializer.Deserialize<UserList>(readText) ?? throw new ReadFileException("Error in reading file");
            User searched = userList.Users.Find(x => x.Id == id);
            userList.Users.Remove(searched);
            userList.Users.Add(entity);
            UserFileSingleton.Instance.RewriteFile(JsonSerializer.Serialize(userList, serializerOptions));
        }

        public void Write(User entity)
        {
            if (UserReader.Instance.UserExists(entity.Login, entity.Mail, entity.Phone))
                throw new WriteFileException("User with current data already exists!");

            string allFileText = UserFileSingleton.Instance.ReadAllFile();
            UserList userList = JsonSerializer.Deserialize<UserList>(allFileText) ?? throw new ReadFileException("Error in reading file");
            userList.Users.Add(entity);
            UserFileSingleton.Instance.RewriteFile(JsonSerializer.Serialize(userList, serializerOptions));
        }

        public void Delete(long id)
        {
            string allFileText = UserFileSingleton.Instance.ReadAllFile();
            UserList userList = JsonSerializer.Deserialize<UserList>(allFileText) ?? throw new ReadFileException("Cannot read file with user");
            User searchedReview = userList.Users.Find(c => c.Id == id) ?? throw new ReadFileException($"There is no user with id {id}");
            userList.Users.Remove(searchedReview);
            string newFileText = JsonSerializer.Serialize(userList);
            UserFileSingleton.Instance.RewriteFile(JsonSerializer.Serialize(userList, serializerOptions));
        }

        private static readonly UserWriter _instance = new UserWriter();

        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
    }
}
