using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Repository;
using System.Text.Json;

namespace Library.Networking.Controller.ControllerImplementation
{
    public class ReviewControllerImpl : IReviewController
    {

        public ReviewControllerImpl(IReviewRepository reviewRepository) { 
            _reviewRepository = reviewRepository;
        }

        public string Delete(long id, out QueryResultFlag queryResult)
        {
            try {
                _reviewRepository.RemoveById(id);
                queryResult = QueryResultFlag.Success;
                return "Deleted successfully!";
            }
            catch (WriteFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (ArgumentNullException)
            {
                queryResult = QueryResultFlag.Error;
                return "Json format error!";
            }
            catch (JsonException)
            {
                queryResult = QueryResultFlag.Error;
                return "Json format error!";
            }
            catch (NotSupportedException)
            {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetAll(out QueryResultFlag queryResult)
        {
            try {
                List<CarReview> carReviews = _reviewRepository.FindAll();
                string reviewsSerialized = JsonSerializer.Serialize(new ReviewList() { Reviews = carReviews });
                queryResult = QueryResultFlag.Success;
                return reviewsSerialized;
            } catch (ReadFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            } catch (NotSupportedException) {
                queryResult = QueryResultFlag.Error;
                return "Functional logic error!";
            }
        }

        public string GetById(long id, out QueryResultFlag queryResult)
        {
            try {
                CarReview review = _reviewRepository.FindById(id);
                string reviewSerialized = JsonSerializer.Serialize(review);
                queryResult = QueryResultFlag.Success;
                return reviewSerialized;
            }
            catch (ReadFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException) {
                queryResult = QueryResultFlag.Error;
                return "Functional logic error!";
            }
        }

        public string GetFromTo(long leftId, long rightId, out QueryResultFlag queryResult)
        {
            try
            {
                List<CarReview> reviews = _reviewRepository.FindFromTo(leftId, rightId);
                string reviewSerialized = JsonSerializer.Serialize(new ReviewList() { Reviews = reviews });
                queryResult = QueryResultFlag.Success;
                return reviewSerialized;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException)
            {
                queryResult = QueryResultFlag.Error;
                return "Functional logic error!";
            }
        }

        public string ReadByCar(long carId, out QueryResultFlag queryResult)
        {
            try
            {
                List<CarReview> reviews = _reviewRepository.ReadByCar(carId);
                string reviewSerialized = JsonSerializer.Serialize(new ReviewList() { Reviews = reviews });
                queryResult = QueryResultFlag.Success;
                return reviewSerialized;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException)
            {
                queryResult = QueryResultFlag.Error;
                return "Functional logic error!";
            }
        }

        public string ReadByUser(long userId, out QueryResultFlag queryResult)
        {
            try
            {
                List<CarReview> reviews = _reviewRepository.ReadByUser(userId);
                string reviewSerialized = JsonSerializer.Serialize(new ReviewList() { Reviews = reviews });
                queryResult = QueryResultFlag.Success;
                return reviewSerialized;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException)
            {
                queryResult = QueryResultFlag.Error;
                return "Functional logic error!";
            }
        }

        public string Save(string jsonQuery, out QueryResultFlag queryResult)
        {
            try {
                CarReview review = JsonSerializer.Deserialize<CarReview>(jsonQuery);
                if (review == null) {
                    queryResult = QueryResultFlag.Error;
                    return "Json format error!";
                }
                _reviewRepository.Save(review);
                queryResult = QueryResultFlag.Success;
                return "Added successfully!";
            }
            catch (WriteFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (ArgumentNullException)
            {
                queryResult = QueryResultFlag.Error;
                return "Json format error!";
            }
            catch (JsonException)
            {
                queryResult = QueryResultFlag.Error;
                return "Json format error!";
            }
            catch (NotSupportedException)
            {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        private IReviewRepository _reviewRepository;
    }
}
