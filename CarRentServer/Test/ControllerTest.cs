using Library.Entities;
using Library.Entities.Collections;
using Library.Networking.Controller;
using Library.Networking.Controller.ControllerImplementation;
using Library.Repository;
using Library.Repository.Implementation;
using System.Text.Json;

namespace Test
{
    public class ControllerTest
    {
        [SetUp]
        public void SetUp() {
            _rents = new Rent[] { new Rent(1, _cars[2], _users[0], new DateTime(2025, 3, 10, 11, 30, 30), new DateTime(2025, 4, 1, 11, 25, 15)),
                                 new Rent(2, _cars[0], _users[2], new DateTime(2025, 3, 17, 8, 30, 48), new DateTime(2025, 3, 17, 15, 17, 43)),
                                 new Rent(3, _cars[0], _users[1], new DateTime(2025, 3, 20, 10, 15, 28), new DateTime(2025, 3, 25, 11, 13, 15)),
                                 new Rent(4, _cars[3], _users[0], new DateTime(2025, 4, 20, 17, 17, 31), new DateTime(2025, 4, 27, 15, 18, 26)),
                                 new Rent(5, _cars[5], _users[1], new DateTime(2025, 4, 15, 11, 11, 56), new DateTime(2025, 4, 16, 10, 8, 11)),
                                 new Rent(6, _cars[5], _users[3], new DateTime(2025, 4, 16, 17, 10, 45), new DateTime(2025, 4, 19, 8, 13, 10)),
                                 new Rent(7, _cars[4], _users[4], new DateTime(2025, 4, 21, 10, 15, 35), new DateTime(2025, 4, 23, 14, 0, 17))
            };

            _reviews = new CarReview[] { new CarReview(1, 2, 1, 4, "Review text one"),
                                        new CarReview(2, 2, 2, 3, "Review text two"),
                                        new CarReview(3, 1, 1, 4, "Review text three"),
                                        new CarReview(4, 3, 1, 4, "Review text four"),
                                        new CarReview(5, 3, 2, 1, "Review text five"),
                                        new CarReview(6, 2, 4, 3, "Review text six"),
                                        new CarReview(7, 3, 1, 5, "Review text seven"),
                                        new CarReview(8, 5, 2, 4, "Review text eight"),
                                        new CarReview(9, 2, 3, 3, "Review text nine"),
                                        new CarReview(10, 4, 1, 2, "Review text ten"),
                                        new CarReview(11, 6, 1, 2, "Review text eleven"),
                                        new CarReview(12, 7, 1, 2, "Review text tvelve"),
                                        new CarReview(13, 8, 2, 3, "Review text thirteen"),
                                        new CarReview(14, 9, 2, 2, "Review text fourteen"),
                                        new CarReview(15, 10, 3, 4, "Review text fifteen")
            };
            _carRepository = new CarRepositoryImpl();
            _rentRepository = new RentRepositoryImpl();
            _reviewRepository = new ReviewRepositoryImpl();
            _userRepository = new UserRepository();
            _manufacturerRepository = new ManufacturerRepository();
            _carController = new CarControllerImpl(_carRepository, _rentRepository, _manufacturerRepository);
            _rentController = new RentControllerImpl(_rentRepository);
            _reviewController = new ReviewControllerImpl(_reviewRepository);
            _userController = new UserControllerImpl(_userRepository);

            foreach (var user in _users)
            {
                _userController.Save(JsonSerializer.Serialize(user), out QueryResultFlag queryResult);
            }

            foreach (var car in _cars)
            {
                _carController.Save(JsonSerializer.Serialize(car), out QueryResultFlag queryResult);
            }

            foreach (var rent in _rents)
            {
                _rentController.Save(JsonSerializer.Serialize(rent), out QueryResultFlag queryResult);
            }

            foreach (var review in _reviews)
            {
                _reviewController.Save(JsonSerializer.Serialize(review), out QueryResultFlag queryResult);
            }
        }

        [Test]
        public void TestAddingEntities() {
            int randomCarId = _random.Next(1, _cars.Length + 1);
            Car selectedCar = _cars.ToList().Find(c => c.Id == randomCarId);
            Car readCar = JsonSerializer.Deserialize<Car>(_carController.GetById(randomCarId, out QueryResultFlag resultFlag));
            Assert.IsTrue(AreTheSame(selectedCar, readCar));
        }

        [Test]
        public void TestCarFiltrationOne() {
            FilterJson filter = new FilterJson(null, null, 15.0f, 25.0f, null, null, null, null);
            string filtratedCars = _carController.GetCarsByFiltration(JsonSerializer.Serialize(filter), out QueryResultFlag queryResult);
            CarList carList = JsonSerializer.Deserialize<CarList>(filtratedCars);
            Assert.That(queryResult, Is.EqualTo(QueryResultFlag.Success));
            var selectedCars = _cars.ToList().FindAll(c => c.Price > 15.0f && c.Price < 25.0f);
            selectedCars.Sort();
            carList.Cars.Sort();
            for (int i = 0; i < selectedCars.Count; i++) {
                Assert.That(carList.Cars[i], Is.EqualTo(selectedCars[i]));
            }
        }

        [Test]
        public void TestCarFiltrationTwo() {
            FilterJson filter = new FilterJson(null, null, 15.0f, 25.0f, "Sedan", null, null, null);
            string filtratedCars = _carController.GetCarsByFiltration(JsonSerializer.Serialize(filter), out QueryResultFlag queryResult);
            CarList carList = JsonSerializer.Deserialize<CarList>(filtratedCars);
            Assert.That(queryResult, Is.EqualTo(QueryResultFlag.Success));
            var selectedCars = _cars.ToList().FindAll(c => c.Price > 10.0f && c.Price < 20.0f && c.Model.Body.Equals("Sedan"));
            selectedCars.Sort();
            carList.Cars.Sort();
            for (int i = 0; i < selectedCars.Count; i++)
            {
                Assert.That(carList.Cars[i], Is.EqualTo(selectedCars[i]));
            }
        }

        [Test]
        public void TestTakingFreeCars() {
            DateTime leftDate = new DateTime(2025, 3, 25, 1, 0, 0);
            DateTime rightDate = new DateTime(2025, 4, 17, 12, 12, 12);
            CarList readCars = JsonSerializer.Deserialize<CarList>(_carController.GetFreeCars(leftDate, rightDate, out QueryResultFlag queryResult));
            List<Rent> rentsInRange = _rents.ToList().FindAll(r => (r.StartRent < leftDate && r.EndRent < leftDate) ||
                                                                    (r.StartRent > rightDate && r.EndRent > leftDate));
            List<Car> selectedCars = new List<Car>();
            foreach (var r in rentsInRange) {
                selectedCars.Add(_cars.ToList().Find(c => c.Id == r.CarId));
            }
            readCars.Cars.Sort();
            selectedCars.Sort();

            for (int i = 0; i < readCars.Cars.Count; i++)
            {
                Assert.That(readCars.Cars[i], Is.EqualTo(selectedCars[i]));
            }
        }

        [Test]
        public void TestAddingRent() {
            int rentsNumber = JsonSerializer.Serialize(_rentController.GetAll(out QueryResultFlag getAllFlag)).Count();
            Rent newRent = new Rent(0, 5, 5, new DateTime(2025, 4, 18, 15, 17, 0), new DateTime(2025, 4, 25, 12, 0, 0));
            string errorMessage = _rentController.Save(JsonSerializer.Serialize(newRent), out QueryResultFlag resultFlag);
            Assert.That(resultFlag, Is.EqualTo(QueryResultFlag.Error));
            int newRentsNumber = JsonSerializer.Serialize(_rentController.GetAll(out getAllFlag)).Count();
            Assert.That(rentsNumber, Is.EqualTo(newRentsNumber));
        }

        private Car[] _cars = new Car[] { new Car(1, new CarModel("Sedan", "Toyota", "Corolla"), 20.0f),
                                         new Car(2, new CarModel("Hatchback", "Volkswagen", "Golf"), 14.0f),
                                         new Car(3, new CarModel("OffRoader", "BMW", "X5"), 45.0f),
                                         new Car(4, new CarModel("Crossover", "Mazda", "CX-5"), 35.0f),
                                         new Car(5, new CarModel("Coupe", "Ford", "Mustang"), 10.0f),
                                         new Car(6, new CarModel("Sedan", "Mercedes", "E-Class"), 45.0f),
                                         new Car(7, new CarModel("Crossover", "Honda", "CV-R"), 30.0f),
                                         new Car(8, new CarModel("OffRoader", "Kia", "Sportage"), 35.0f),
                                         new Car(9, new CarModel("Universal", "Audi", "Avant"), 15.0f),
                                         new Car(10, new CarModel("Cabriolet", "Chevrolet", "Camaro"), 70.0f),
                                         new Car(11, new CarModel("Sedan", "Honda", "Civic"), 20.0f),
                                         new Car(12, new CarModel("Hatchback", "Audi", "A3 Sportback"), 24.0f),
                                         new Car(13, new CarModel("Sedan", "Toyota", "Camry"), 20.0f),
                                         new Car(14, new CarModel("Hatchback", "Volkswagen", "Golf GTI"), 18.0f), 
                                         new Car(15, new CarModel("Hatchback", "Ford", "Focus"), 26.0f) };

        [TearDown]
        public void TearDown()
        {
            _carRepository.ClearAll();
            _rentRepository.ClearAll();
            _reviewRepository.ClearAll();
            _userRepository.ClearAll();
        }

        private User[] _users = new User[] { new User(1, "First", "UserOne", "+375-33-1111111", "FirstUser@gmail.com", "PasswordOne"),
                                            new User(2, "Second", "UserTwo", "+375-33-2222222", "SecondUser@gmail.com", "PasswordTwo"),
                                            new User(3, "Third", "UserThree", "+375-33-3333333", "ThirdUser@gmail.com", "PasswordThree"),
                                            new User(4, "Fourth", "UserFour", "+375-33-4444444", "FourthUser@gmail.com", "PasswordFour"),
                                            new User(5, "Fifth", "UserFive", "+375-33-5555555", "FifthUser@gmail.com", "PasswordFive")
        };

        private bool AreTheSame(Car carOne, Car carTwo)
        {
            return carOne.Id == carTwo.Id
                && carOne.Model.Mark == carTwo.Model.Mark
                && carOne.Model.Name.Equals(carTwo.Model.Name)
                && carOne.Model.Body == carTwo.Model.Body
                && carOne.Price == carTwo.Price;
        }

        private bool AreTheSame(User userOne, User userTwo)
        {
            return userOne.Name.Equals(userTwo.Name) && userOne.Id == userTwo.Id && userOne.Mail.Equals(userTwo.Mail) && userOne.Login.Equals(userTwo.Login) &&
                userOne.Password.Equals(userTwo.Password) && userOne.Password.Equals(userTwo.Password);
        }

        private bool AreTheSame(Rent rentOne, Rent rentTwo)
        {
            return rentOne.StartRent.Equals(rentTwo.StartRent) && rentOne.EndRent.Equals(rentTwo.EndRent) && rentOne.Id == rentTwo.Id && rentOne.UserId == rentTwo.UserId;
        }

        private bool AreTheSame(CarReview reviewOne, CarReview reviewTwo)
        {
            return reviewOne.Rate == reviewTwo.Rate && reviewOne.Id == reviewTwo.Id && reviewOne.UserId == reviewTwo.UserId && reviewOne.CarId == reviewTwo.CarId
                && reviewOne.Text.Equals(reviewTwo.Text);
        }


        private Rent[] _rents;

        private CarReview[] _reviews;

        private Random _random = new Random();


        private ICarRepository _carRepository;
        private IRentRepository _rentRepository;
        private IReviewRepository _reviewRepository;
        private IUserRepository _userRepository;
        private ICarController _carController;
        private IRentController _rentController;
        private IReviewController _reviewController;
        private IUserController _userController;
        private IManufacturerRepository _manufacturerRepository;
    }
}
