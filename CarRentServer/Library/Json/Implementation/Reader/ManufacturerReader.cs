using CarRentEntities.Entities.Collections;
using Library.Exception;
using Library.Json.Reader;
using Library.Json.Singletons;
using System.Text.Json;

namespace Library.Json.Implementation.Reader
{
    public class ManufacturerReader : IManufacturerReader
    {

        private static readonly ManufacturerReader instance = new ManufacturerReader();

        private ManufacturerReader() { }

        public static ManufacturerReader Instance { get { return instance; } }
        public List<string> ReadAll()
        {
            string carFileText = ManufacturerFileSingleton.Instance.ReadAllFile();
            CarManufacturerList carList = JsonSerializer.Deserialize<CarManufacturerList>(carFileText) ?? 
                                            throw new ReadFileException("No data about manufacturers.");
            return carList.ManufacturerMarks;
        }
    }
}
