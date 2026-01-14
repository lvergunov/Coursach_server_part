using Library.Entities;
using Library.Json.Implementation.Reader;
using Library.Json.Implementation.Writter;

namespace Library.Repository.Implementation
{
    public class CarRepositoryImpl : ICarRepository
    {
        public void ClearAll()
        {
            CarWriter.Instance.ResetData();
        }

        public List<Car> Filter(float? lowCost, float? highCost, string? carBody, string? manufacturer, string name, bool? active)
        {
            return CarReader.Instance.Filter(lowCost, highCost, carBody, manufacturer, name, active);
        }

        public List<Car> FindAll()
        {
            return CarReader.Instance.ReadAllObjects();
        }

        public List<Car> FindByCarBody(string body)
        {
            return CarReader.Instance.ReadByCarBody(body);
        }

        public List<Car> FindByCarManufacturer(string manufacturer)
        {
            return CarReader.Instance.ReadByMark(manufacturer);
        }

        public Car FindById(long id)
        {
            return CarReader.Instance.ReadOneObject(id);
        }

        public List<Car> FindCarByActivity(bool activity)
        {
            return CarReader.Instance.ReadCarsByActivity(activity);
        }

        public List<Car> FindFromTo(long leftId, long rightId)
        {
            return CarReader.Instance.ReadFromTo(leftId, rightId);
        }

        public List<Car> ReadByModelName(string modelName)
        {
            return CarReader.Instance.ReadByModelName(modelName);
        }

        public List<Car> ReadByPriceInRate(float lowerCost, float higherCost)
        {
            return CarReader.Instance.ReadByPriceBetween(lowerCost, higherCost);
        }

        public void RemoveById(long id)
        {
            CarWriter.Instance.Delete(id);
        }

        public void Save(Car entity)
        {
            List<Car> allCars = CarReader.Instance.ReadAllObjects();
            if (allCars.Any(c => c.Id == entity.Id))
            {
                CarWriter.Instance.Update(entity.Id, entity);
            }
            else {
                long biggestId = 1;
                if (allCars.Count != 0)
                    biggestId = allCars.Max(c => c.Id) + 1;
                CarWriter.Instance.Write(new Car(biggestId, entity.Model, entity.Price, entity.Rate, entity.NumberOfRates));
            }
        }
    }
}
