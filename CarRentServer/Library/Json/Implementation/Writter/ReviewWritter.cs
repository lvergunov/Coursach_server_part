using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Implementation.Reader;
using Library.Json.Singletons;
using Library.Json.Writter;
using System.Text.Json;

namespace Library.Json.Implementation.Writter
{
    public class ReviewWritter : IReviewWriter
    {
        private static readonly ReviewWritter instance = new ReviewWritter();

        public static ReviewWritter Instance { get { return instance; } }

        private ReviewWritter() { }

        public void Update(long id, CarReview entity)
        {
            string reviewFileText = ReviewFileSingleton.Instance.ReadAllFile();
            ReviewList allReviews = JsonSerializer.Deserialize<ReviewList>(reviewFileText) ?? throw new ReadFileException("Cannot read reviews.");
            allReviews.Reviews.Remove(allReviews.Reviews.Find(r => r.Id == id) ?? throw new ReadFileException($"There is no entity with id {id}"));
            allReviews.Reviews.Add(entity);
            ResetCarRate(allReviews, entity.CarId);
            string newFileText = JsonSerializer.Serialize(allReviews, serializerOptions);
            ReviewFileSingleton.Instance.RewriteFile(newFileText);
        }

        public void Write(CarReview entity)
        {
            string reviewFileText = ReviewFileSingleton.Instance.ReadAllFile();
            ReviewList allReviews = JsonSerializer.Deserialize<ReviewList>(reviewFileText) ?? throw new ReadFileException("Cannot read reviews.");
            allReviews.Reviews.Add(entity);
            ResetCarRate(allReviews, entity.CarId);
            string newText = JsonSerializer.Serialize(allReviews, serializerOptions);
            ReviewFileSingleton.Instance.RewriteFile(newText);
        }

        private void ResetCarRate(ReviewList allReviews, long carId) {
            List<float> rates = allReviews.Reviews.FindAll(cr => cr.CarId == carId).Select(r => (float)r.Rate).ToList();
            
            Car searchedCar = CarReader.Instance.ReadOneObject(carId);
            searchedCar = searchedCar.RenewRate(rates);
            CarWriter.Instance.Update((long)searchedCar.Id, searchedCar);
        }

        public void ResetData()
        {
            ReviewFileSingleton.Instance.TearDownAll();
        }

        public void Delete(long id)
        {
            string allFileText = ReviewFileSingleton.Instance.ReadAllFile();
            ReviewList reviewList = JsonSerializer.Deserialize<ReviewList>(allFileText) 
                ?? throw new ReadFileException("Cannot read file with review");
            CarReview searchedReview = reviewList.Reviews.Find(c => c.Id == id) 
                ?? throw new ReadFileException($"There is no review with id {id}");
            reviewList.Reviews.Remove(searchedReview);
            ResetCarRate(reviewList, searchedReview.CarId);
            string newFileText = JsonSerializer.Serialize(reviewList, serializerOptions);
            ReviewFileSingleton.Instance.RewriteFile(newFileText);
        }


        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
    }
}
