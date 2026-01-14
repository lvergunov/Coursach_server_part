using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Singletons;
using Library.Json.Writter;
using System.Text.Json;

namespace Library.Json.Implementation.Writter
{
    public class CarWriter : ICarWriter
    {

        private static readonly CarWriter carWriter = new CarWriter();

        public static CarWriter Instance { get { return carWriter; } }

        private CarWriter() { }
        public void Write(Car entity)
        {
            string fileText = CarFileSingleton.Instance.ReadAllFile();
            CarList carList = JsonSerializer.Deserialize<CarList>(fileText) ?? new CarList();
            carList.Cars.Add(entity);
            string newJson = JsonSerializer.Serialize(carList, serializerOptions);
            CarFileSingleton.Instance.RewriteFile(newJson);
        }

        public void Update(long id, Car entity)
        {
            string fileText = CarFileSingleton.Instance.ReadAllFile();
            CarList carList = JsonSerializer.Deserialize<CarList>(fileText) ?? new CarList();
            Car entityToUpdate = carList.Cars.Find(x => x.Id == id) ?? throw new WriteFileException($"There is no car with id {id}");
            carList.Cars.Remove(entityToUpdate);
            carList.Cars.Add(entity);
            string newJson = JsonSerializer.Serialize(carList, serializerOptions);
            CarFileSingleton.Instance.RewriteFile(newJson);
        }

        public void ResetData()
        {
            CarFileSingleton.Instance.TearDownAll();
        }

        public void Delete(long id)
        {
            string allFileText = CarFileSingleton.Instance.ReadAllFile();
            CarList carList = JsonSerializer.Deserialize<CarList>(allFileText) ?? throw new ReadFileException("Cannot read file with cars");
            Car searchedCar = carList.Cars.Find(c => c.Id == id) ?? throw new ReadFileException($"There is no car with id {id}");
            carList.Cars.Remove(searchedCar);
            string newFileText = JsonSerializer.Serialize(carList, serializerOptions);
            CarFileSingleton.Instance.RewriteFile(newFileText);
        }

        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
    }
}
