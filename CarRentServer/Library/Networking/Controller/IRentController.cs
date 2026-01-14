namespace Library.Networking.Controller
{
    public interface IRentController : ICommonController
    {
        public string GetRentsForCar(long carId, QueryResultFlag queryResult);
        public string GetForCarBetweenDates(long carId, DateTime leftDate, DateTime rightDate, QueryResultFlag queryResult);
        public string GetForUser(long userId, out QueryResultFlag queryResult);
        public string GetForUserBetweenDates(long userId, DateTime startDate, DateTime endDate, out QueryResultFlag queryResult);
    }
}
