using Library.Entities;

namespace Library.Repository
{
    public interface IReviewRepository : ICommonRepository<CarReview>
    {
        public List<CarReview> ReadByUser(long userId);

        public List<CarReview> ReadByCar(long carId);
    }
}
