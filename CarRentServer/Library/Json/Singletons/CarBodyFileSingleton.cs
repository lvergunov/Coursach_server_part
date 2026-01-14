using Library.Entities.Collections;
using System.Text.Json;

namespace Library.Json.Singletons
{
    public class CarBodyFileSingleton : CommonSingleton
    {
        public CarBodyFileSingleton() : base(FILE_PATH)
        {
        }

        private static readonly CarBodyFileSingleton _instance = new CarBodyFileSingleton();

        public static CarBodyFileSingleton Instance { get { return _instance; } }
        
        protected override void InitializeFile()
        {
            string initString = JsonSerializer.Serialize(new CarBodyList());
            File.WriteAllText(fullFilePath, initString);
        }

        private const string FILE_PATH = "CarBodies.json";
    }
}
