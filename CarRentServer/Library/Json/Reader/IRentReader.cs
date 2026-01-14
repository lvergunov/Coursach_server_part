using Library.Entities;

namespace Library.Json.Reader
{
    public interface IRentReader : IJsonReader<Rent>
    {
        List<Rent> ReadForCar(long carId);

        List<Rent> ReadForCarBetweenDates(long carId, DateTime startDate, DateTime endDate);

        List<Rent> ReadForUser(long userId);

        List<Rent> ReadForUserBetweenDates(long userId, DateTime startDate, DateTime endDate);
    }
}
