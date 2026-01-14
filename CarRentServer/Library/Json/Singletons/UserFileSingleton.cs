using Library.Entities.Collections;
using System.Text.Json;

namespace Library.Json.Singletons
{
    public class UserFileSingleton : CommonSingleton
    {
        private static readonly UserFileSingleton _instance = new UserFileSingleton();

        private const string FILE_PATH = "Users.json";
        public static UserFileSingleton Instance { get { return _instance; } }

        public UserFileSingleton() : base(FILE_PATH)
        {
        }

        protected override void InitializeFile()
        {
            string initialString = JsonSerializer.Serialize(new UserList());
            File.WriteAllText(fullFilePath, initialString);
        }
    }
}
