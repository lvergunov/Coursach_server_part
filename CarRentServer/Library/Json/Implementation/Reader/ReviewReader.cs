using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Reader;
using Library.Json.Singletons;
using System.Text.Json;

namespace Library.Json.Implementation.Reader
{
    public class ReviewReader : IReviewReader
    {
        private static readonly ReviewReader instance = new ReviewReader();

        public static ReviewReader Instance { get { return instance; } }

        private ReviewReader() { }

        public List<CarReview> ReadAllObjects()
        {
            string reviewFileText = ReviewFileSingleton.Instance.ReadAllFile();
            ReviewList allReviews = JsonSerializer.Deserialize<ReviewList>(reviewFileText) ?? throw new ReadFileException("Cannot read reviews.");
            return allReviews.Reviews;
        }

        public List<CarReview> ReadByCar(long carId)
        {
            return ReadAllObjects().FindAll(cr => cr.CarId == carId);
        }

        public List<CarReview> ReadByUser(long userId)
        {
            return ReadAllObjects().FindAll(cr => cr.UserId == userId);
        }

        public List<CarReview> ReadFromTo(long startId, long finishId)
        {
            return ReadAllObjects().FindAll(cr => cr.Id <= finishId && cr.Id >= startId);
        }

        public CarReview ReadOneObject(long id)
        {
            return ReadAllObjects().Find(cr => cr.Id == id) ?? throw new ReadFileException($"There is no entity with id {id}");
        }
    }
}
