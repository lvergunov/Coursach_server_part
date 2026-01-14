using Library.Entities;

namespace Library.Repository
{
    public interface ICommonRepository<E> where E : CommonEntity
    {
        void Save(E entity);

        E FindById(long id);

        List<E> FindAll();

        List<E> FindFromTo(long leftId, long rightId);

        void RemoveById(long id);

        void ClearAll();
    }
}
