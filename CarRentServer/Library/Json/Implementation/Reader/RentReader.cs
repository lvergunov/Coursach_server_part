using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Reader;
using Library.Json.Singletons;
using System.Text.Json;

namespace Library.Json.Implementation.Reader
{
    public class RentReader : IRentReader
    {
        private static readonly RentReader _instance = new RentReader();

        public static RentReader Instance { get { return _instance; } }

        private RentReader() { }
        public List<Rent> ReadAllObjects()
        {
            string fileText = RentFileSingleton.Instance.ReadAllFile();
            RentList rentList = JsonSerializer.Deserialize<RentList>(fileText) ?? throw new ReadFileException("Cannot read file with rents.");
            rentList.Rents.ForEach(r => DeserializeInnerObjects(r));
            return rentList.Rents;
        }

        public List<Rent> ReadForCar(long carId)
        {
            return ReadAllObjects().FindAll(r => r.CarId == carId);
        }

        public List<Rent> ReadForCarBetweenDates(long carId, DateTime startDate, DateTime endDate)
        {
            return ReadAllObjects().FindAll(r => r.CarId == carId && r.StartRent >= startDate && r.EndRent <= endDate);
        }

        public List<Rent> ReadForUser(long userId)
        {
            return ReadAllObjects().FindAll(r => r.UserId == userId);
        }

        public List<Rent> ReadForUserBetweenDates(long userId, DateTime startDate, DateTime endDate)
        {
            return ReadAllObjects().FindAll(r => r.UserId == userId && r.StartRent >= startDate && r.EndRent <= endDate);
        }

        public List<Rent> ReadFromTo(long startId, long finishId)
        {
            return ReadAllObjects().FindAll(r => r.Id >= startId && r.Id <= finishId);
        }

        public Rent ReadOneObject(long id)
        {
            return ReadAllObjects().Find(r => r.Id == id) ?? throw new ReadFileException($"Cannot find rent with id {id}");
        }

        private void DeserializeInnerObjects(Rent rent) {
            rent.SetInnerObjects(CarReader.Instance.ReadOneObject(rent.CarId), 
                UserReader.Instance.ReadOneObject(rent.UserId));
        }
    }
}
