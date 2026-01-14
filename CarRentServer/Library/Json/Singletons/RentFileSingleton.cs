using Library.Entities.Collections;
using System.Text.Json;

namespace Library.Json.Singletons
{
    public class RentFileSingleton : CommonSingleton
    {

        private const string FILE_PATH = "Rents.json";
        private RentFileSingleton() : base(FILE_PATH){ }

        private static readonly RentFileSingleton _instance = new RentFileSingleton();

        public static RentFileSingleton Instance { get { return _instance; } }

        protected override void InitializeFile()
        {
            string initialString = JsonSerializer.Serialize(new RentList());
            File.WriteAllText(fullFilePath, initialString);
        }
    }
}
