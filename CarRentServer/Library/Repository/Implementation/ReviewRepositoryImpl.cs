using Library.Entities;
using Library.Json.Implementation.Reader;
using Library.Json.Implementation.Writter;

namespace Library.Repository.Implementation
{
    public class ReviewRepositoryImpl : IReviewRepository
    {
        public void ClearAll()
        {
            ReviewWritter.Instance.ResetData();
        }

        public List<CarReview> FindAll()
        {
            return ReviewReader.Instance.ReadAllObjects();
        }

        public CarReview FindById(long id)
        {
            return ReviewReader.Instance.ReadOneObject(id);
        }

        public List<CarReview> FindFromTo(long leftId, long rightId)
        {
            return ReviewReader.Instance.ReadFromTo(leftId, rightId);
        }

        public List<CarReview> ReadByCar(long carId)
        {
            return ReviewReader.Instance.ReadByCar(carId);
        }

        public List<CarReview> ReadByUser(long userId)
        {
            return ReviewReader.Instance.ReadByUser(userId);
        }

        public void RemoveById(long id)
        {
            ReviewWritter.Instance.Delete(id);
        }

        public void Save(CarReview entity)
        {
            List<CarReview> allRewievs = ReviewReader.Instance.ReadAllObjects();
            if (allRewievs.Any(c => c.Id == entity.Id))
            {
                ReviewWritter.Instance.Update(entity.Id, entity);
            }
            else
            {
                long biggestId = 1;
                if (allRewievs.Count != 0)
                    biggestId = allRewievs.Max(c => c.Id) + 1;
                ReviewWritter.Instance.Write(new CarReview(biggestId, entity.CarId, entity.UserId, entity.Rate, entity.Text));
            }
        }
    }
}
