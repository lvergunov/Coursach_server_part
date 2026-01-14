using Library.Entities;

namespace Library.Networking.Controller
{
    public interface ICommonController
    {
        public string GetAll(out QueryResultFlag queryResult);
        public string GetFromTo(long leftId, long rightId, out QueryResultFlag queryResult);
        public string GetById(long id, out QueryResultFlag queryResult);

        public string Save(string jsonQuery, out QueryResultFlag queryResult);
        public string Delete(long id, out QueryResultFlag queryResult);
    }
}
