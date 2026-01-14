using Library.Entities;

namespace Library.Json.Reader
{
    public interface IJsonReader <E> where E : CommonEntity
    {
        E ReadOneObject(long id);
        List<E> ReadAllObjects();
        List<E> ReadFromTo(long startId, long finishId);
    }
}
