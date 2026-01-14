using Library.Entities;

namespace Library.Json.Reader
{
    public interface IReviewReader : IJsonReader<CarReview>
    {
        public List<CarReview> ReadByUser(long userId);

        public List<CarReview> ReadByCar(long carId);
    }
}
