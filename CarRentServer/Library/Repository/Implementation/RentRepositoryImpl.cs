using Library.Entities;
using Library.Exception;
using Library.Json.Implementation.Reader;
using Library.Json.Implementation.Writter;

namespace Library.Repository.Implementation
{
    public class RentRepositoryImpl : IRentRepository
    {
        public void ClearAll()
        {
            RentWriter.Instance.ResetData();
        }

        public List<Rent> FindAll()
        {
            return RentReader.Instance.ReadAllObjects();
        }

        public Rent FindById(long id)
        {
            return RentReader.Instance.ReadOneObject(id);
        }

        public List<Rent> FindFromTo(long leftId, long rightId)
        {
            return RentReader.Instance.ReadFromTo(leftId, rightId);
        }

        public List<Rent> ReadForCar(long carId)
        {
            return RentReader.Instance.ReadForCar(carId);
        }

        public List<Rent> ReadForCarBetweenDates(long carId, DateTime startDate, DateTime endDate)
        {
            return RentReader.Instance.ReadForCarBetweenDates(carId, startDate, endDate);
        }

        public List<Rent> ReadForUser(long userId)
        {
            return RentReader.Instance.ReadForUser(userId);
        }

        public List<Rent> ReadForUserBetweenDates(long userId, DateTime startDate, DateTime endDate)
        {
            return RentReader.Instance.ReadForUserBetweenDates(userId, startDate, endDate);
        }

        public void RemoveById(long id)
        {
            RentWriter.Instance.Delete(id);
        }

        public void Save(Rent entity)
        {
            List<Rent> allRents = RentReader.Instance.ReadAllObjects();
            if (allRents.Any(r => r.Id == entity.Id))
            {
                RentWriter.Instance.Update(entity.Id, entity);
            }
            else
            {
                foreach (var rent in allRents) {
                    if(AreDatesCrossed(entity, rent.StartRent, rent.EndRent) && entity.CarId == rent.CarId)
                        throw new CarIsBuisyException($"This car is buisy since {rent.StartRent} till {rent.EndRent}.");
                }
                long biggestId = 1;
                if (allRents.Count != 0)
                    biggestId = allRents.Max(c => c.Id) + 1;
                RentWriter.Instance.Write(new Rent(biggestId, entity.CarId, entity.UserId, entity.StartRent, entity.EndRent));
            }
        }

        public bool AreDatesCrossed(Rent entity, DateTime leftDate, DateTime rightDate) { 
            return (leftDate > entity.StartRent && leftDate < entity.EndRent) || 
                (rightDate > entity.StartRent && rightDate < entity.EndRent) ||
                (leftDate < entity.StartRent && rightDate > entity.EndRent);
        }
    }
}
