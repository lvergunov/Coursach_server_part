namespace Library.Networking.Controller
{
    public interface ICarController : ICommonController
    {
        public string GetFreeCars(DateTime leftDate, DateTime rightDate, out QueryResultFlag queryResult);
        public string GetCarsByFiltration(string jsonParams, out QueryResultFlag queryResult);
        public string GetCarBodies(out QueryResultFlag queryResult);

        public string GetCarManufacturers(out QueryResultFlag queryResult);
        public string GetActiveNow(out QueryResultFlag queryResult);
        public string GetAllByUserRents(long userId, out QueryResultFlag queryResult);
    }
}
