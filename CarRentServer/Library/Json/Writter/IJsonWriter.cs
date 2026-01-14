using Library.Entities;

namespace Library.Json.Writter
{
    public interface IJsonWriter<E> where E : CommonEntity
    {
        void Write(E entity);

        void Update(long id, E entity);

        void ResetData();

        void Delete(long id);
    }
}
