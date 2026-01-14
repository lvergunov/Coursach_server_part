using Library.Entities.Collections;
using System.Text.Json;

namespace Library.Json.Singletons
{
    internal class CarFileSingleton : CommonSingleton
    {
        private const string FILE_PATH = "Cars.json";

        private CarFileSingleton() : base(FILE_PATH) { }

        private static readonly CarFileSingleton _instance = new CarFileSingleton();

        public static CarFileSingleton Instance { get { return _instance; } }

        protected override void InitializeFile()
        {
            string initString = JsonSerializer.Serialize(new CarList());
            File.WriteAllText(fullFilePath, initString);
        }
    }
}
