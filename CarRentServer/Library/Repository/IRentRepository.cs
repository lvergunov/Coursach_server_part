using Library.Entities;

namespace Library.Repository
{
    public interface IRentRepository : ICommonRepository<Rent>
    {
        List<Rent> ReadForCar(long carId);
        List<Rent> ReadForCarBetweenDates(long carId, DateTime startDate, DateTime endDate);
        List<Rent> ReadForUser(long userId);
        List<Rent> ReadForUserBetweenDates(long userId, DateTime startDate, DateTime endDate);
        public bool AreDatesCrossed(Rent entity, DateTime leftDate, DateTime rightDate);
    }
}
