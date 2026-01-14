using CarRentEntities.Entities.Collections;
using System.Text.Json;

namespace Library.Json.Singletons
{
    public class ManufacturerFileSingleton : CommonSingleton
    {
        public static ManufacturerFileSingleton Instance { get { return _instance;  } }

        public ManufacturerFileSingleton() : base(FILE_PATH)
        {
        }

        protected override void InitializeFile()
        {
            string initString = JsonSerializer.Serialize(new CarManufacturerList());
            File.WriteAllText(fullFilePath, initString);
        }

        private static readonly ManufacturerFileSingleton _instance = new ManufacturerFileSingleton();

        private const string FILE_PATH = "Manufacturers.json";
    }
}
