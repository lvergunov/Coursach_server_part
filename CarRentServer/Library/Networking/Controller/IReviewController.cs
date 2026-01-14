namespace Library.Networking.Controller
{
    public interface IReviewController : ICommonController
    {
        public string ReadByCar(long carId, out QueryResultFlag queryResult);
        public string ReadByUser(long userId, out QueryResultFlag queryResult);
    }
}
