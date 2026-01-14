using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Reader;
using Library.Json.Singletons;
using System.Text.Json;

namespace Library.Json.Implementation.Reader
{
    public class CarReader : ICarReader
    {

        private static readonly CarReader instance = new CarReader();

        private CarReader() { }

        public static CarReader Instance { get { return instance; } }

        public List<Car> Filter(float? lowCost, float? highCost, string? carBody, string? manufacturer, string name, bool? active)
        {
            List<List<Car>> filterSlices = new List<List<Car>>();
            if (lowCost != null) {
                filterSlices.Add(ReadByPriceBetween(lowCost.Value, float.MaxValue));
            }
            if (highCost != null) {
                filterSlices.Add(ReadByPriceBetween(0.0f, highCost.Value));
            }
            if (carBody != null) {
                filterSlices.Add(ReadByCarBody(carBody));
            }
            if (manufacturer != null) {
                filterSlices.Add(ReadByMark(manufacturer));
            }
            if (name != null) {
                filterSlices.Add(ReadByModelName(name));
            }
            //if (active != null) {
            //    filterSlices.Add(ReadCarsByActivity(active.Value));
            //}

            List<Car> allCars = ReadAllObjects();
            foreach (var carFilter in filterSlices) {
                allCars = allCars.Intersect(carFilter).ToList();
            }
            return allCars;
        }

        public List<Car> ReadCarsByActivity(bool active)
        {
            List<Car> list = new List<Car>();
            string rentFileText = RentFileSingleton.Instance.ReadAllFile();
            RentList rentList = JsonSerializer.Deserialize<RentList>(rentFileText) ?? throw new ReadFileException("No data about rent.");
            List<Car> carList = ReadAllObjects();
            var activeRents = rentList.Rents.Where(c => c.IsActive == active).ToList();
            foreach (var rent in activeRents) {
                list.Add(carList.Find(c => c.Id == rent.CarId) ?? throw new ReadFileException("No active rents"));
            }
            return list;
        }

        public List<Car> ReadAllObjects()
        {
            string carFileText = CarFileSingleton.Instance.ReadAllFile();
            CarList carList = JsonSerializer.Deserialize<CarList>(carFileText) ?? throw new ReadFileException("No data about cars.");
            return carList.Cars;
        }

        public List<Car> ReadByCarBody(string carBody)
        {
            List<Car> allCars = ReadAllObjects();
            return allCars.FindAll(c => c.Model.Body.Equals(carBody));
        }

        public List<Car> ReadByMark(string mark)
        {
            List<Car> allCars = ReadAllObjects();
            return allCars.FindAll(c => c.Model.Mark.Equals(mark));
        }

        public List<Car> ReadByModelName(string name)
        {
            List<Car> allCars = ReadAllObjects();
            return allCars.FindAll(c => c.Model.Name.Equals(name));
        }

        public List<Car> ReadByPriceBetween(float lowCost, float highCost)
        {
            List<Car> allCars = ReadAllObjects();
            return allCars.FindAll(c => c.Price > lowCost && c.Price < highCost);
        }

        public List<Car> ReadFromTo(long startId, long finishId)
        {
            List<Car> allCars = ReadAllObjects();
            return allCars.FindAll(c => c.Id >= startId && c.Id <= finishId);
        }

        public Car ReadOneObject(long id)
        {
            List<Car> allCars = ReadAllObjects();
            return allCars.Find(c => c.Id == id) ?? throw new ReadFileException($"There is no car with id = {id}");
        }
    }
}
