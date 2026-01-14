using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Reader;
using Library.Json.Singletons;
using System.Text.Json;

namespace Library.Json.Implementation.Reader
{
    public class CarBodyReader : ICarBodyReader
    {
        private static readonly CarBodyReader instance = new CarBodyReader();

        private CarBodyReader() { }

        public static CarBodyReader Instance { get { return instance; } }
        public List<string> GetAll()
        {
            string carFileText = CarBodyFileSingleton.Instance.ReadAllFile();
            CarBodyList carBodyList = JsonSerializer.Deserialize<CarBodyList>(carFileText) ??
                                            throw new ReadFileException("No data about manufacturers.");
            return carBodyList.CarBodies;
        }
    }
}
