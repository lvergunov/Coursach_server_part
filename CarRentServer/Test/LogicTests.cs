using Library.Entities;
using Library.Exception;
using Library.Repository;
using Library.Repository.Implementation;

namespace Test
{
    public class LogicTests
    {
        private Car[] cars = new Car[] { new Car(1, new CarModel("Sedan", "Toyota", "Corolla"), 20.0f),
                                         new Car(2, new CarModel("Hatchback", "Volkswagen", "Golf"), 14.0f),
                                         new Car(3, new CarModel("OffRoader", "BMW", "X5"), 45.0f),
                                         new Car(4, new CarModel("Crossover", "Mazda", "CX-5"), 35.0f),
                                         new Car(5, new CarModel("Coupe", "Ford", "Mustang"), 10.0f),
                                         new Car(6, new CarModel("Sedan", "Mercedes", "E-Class"), 45.0f),
                                         new Car(7, new CarModel("Crossover", "Honda", "CV-R"), 30.0f),
                                         new Car(8, new CarModel("OffRoader", "Kia", "Sportage"), 35.0f),
                                         new Car(9, new CarModel("Universal", "Audi", "Avant"), 15.0f),
                                         new Car(10, new CarModel("Cabriolet", "Chevrolet", "Camaro"), 70.0f)};

        private User[] users = new User[] { new User(1, "First", "UserOne", "+375-33-1111111", "FirstUser@gmail.com", "PasswordOne"),
                                            new User(2, "Second", "UserTwo", "+375-33-2222222", "SecondUser@gmail.com", "PasswordTwo"),
                                            new User(3, "Third", "UserThree", "+375-33-3333333", "ThirdUser@gmail.com", "PasswordThree"),
                                            new User(4, "Fourth", "UserFour", "+375-33-4444444", "FourthUser@gmail.com", "PasswordFour"),
                                            new User(5, "Fifth", "UserFive", "+375-33-5555555", "FifthUser@gmail.com", "PasswordFive")
        };

        private Rent[] rents;

        private CarReview[] reviews;

        private Random random = new Random();

        [SetUp]
        public void Setup()
        {
            carRepository = new CarRepositoryImpl();
            rentRepository = new RentRepositoryImpl();
            reviewRepository = new ReviewRepositoryImpl();
            userRepository = new UserRepository();
            cars.ToList().ForEach(c => carRepository.Save(c));

            rents = new Rent[] { new Rent(1, cars[2], users[0], new DateTime(2025, 3, 10, 11, 30, 30), new DateTime(2025, 4, 1, 11, 25, 15)),
                                 new Rent(2, cars[0], users[2], new DateTime(2025, 3, 17, 8, 30, 48), new DateTime(2025, 3, 17, 15, 17, 43)),
                                 new Rent(3, cars[0], users[1], new DateTime(2025, 3, 20, 10, 15, 28), new DateTime(2025, 3, 25, 11, 13, 15)),
                                 new Rent(4, cars[3], users[0], new DateTime(2025, 4, 20, 17, 17, 31), new DateTime(2025, 4, 27, 15, 18, 26)),
                                 new Rent(5, cars[5], users[1], new DateTime(2025, 4, 15, 11, 11, 56), new DateTime(2025, 4, 16, 10, 8, 11)),
                                 new Rent(6, cars[5], users[3], new DateTime(2025, 4, 16, 17, 10, 45), new DateTime(2025, 4, 19, 8, 13, 10)),
                                 new Rent(7, cars[4], users[4], new DateTime(2025, 4, 21, 10, 15, 35), new DateTime(2025, 4, 23, 14, 0, 17))
            };

            reviews = new CarReview[] { new CarReview(1, 2, 1, 4, "Review text one"),
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
            users.ToList().ForEach(u => userRepository.Save(u));
            rents.ToList().ForEach(r => rentRepository.Save(r));
            reviews.ToList().ForEach(rw => reviewRepository.Save(rw));
        }

        [Test]
        public void TestReadAllCars()
        {
            List<Car> readCars = carRepository.FindAll();
            readCars.Sort();
            for (int i = 0; i < cars.Length; i++) {
                Assert.IsTrue(AreTheSame(readCars[i], cars[i]));
            }
        }

        [Test]
        public void TestReadOneCar() {
            long randomIndex = random.Next(1, cars.Length + 1);
            Car readCar = carRepository.FindById(randomIndex);
            Assert.IsTrue(AreTheSame(cars[randomIndex - 1], readCar));
        }

        [Test]
        public void TestReadFromToCars() {
            int leftRandomIndex = random.Next(1, cars.Length / 2 + 1);
            int rightRandomIndex = random.Next(cars.Length / 2, cars.Length + 1);
            List<Car> readCars = carRepository.FindFromTo(leftRandomIndex, rightRandomIndex);
            readCars.Sort();
            List<Car> selectedCars = new List<Car>();
            for (int i = leftRandomIndex - 1; i < rightRandomIndex; i++) {
                selectedCars.Add(cars[i]);
            }
            for (int i = 0; i < readCars.Count; i++) {
                Assert.IsTrue(AreTheSame(readCars[i], selectedCars[i]));
            }
        }

        [Test]
        public void TestReadAllUsers() {
            List<User> readUsers = userRepository.FindAll();
            for (int i = 0; i < readUsers.Count; i++) {
                Assert.IsTrue(AreTheSame(readUsers[i], users[i]));
            }
        }

        [Test]
        public void TestReadOneUser() {
            int randomIndex = random.Next(1, users.Length + 1);
            Assert.IsTrue(AreTheSame(users[randomIndex - 1], userRepository.FindById(randomIndex)));
        }

        [Test]
        public void TestUserVerification() {
            int randomIndex = random.Next(0, users.Length);
            Assert.IsTrue(AreTheSame(users[randomIndex], userRepository.VerifyUser(users[randomIndex].Login, users[randomIndex].Password)));
        }

        [Test]
        public void TestUserAddingExceptionOne() {
            int randomIndex = random.Next(0, users.Length);

            Assert.Throws<UserExistsException>(() => userRepository.Save(new User(6, "User6", users[randomIndex].Login, "Phone6", "Mail6", "Password6")));
        }

        [Test]
        public void TestUserAddingExceptionTwo() {
            int randomIndex = random.Next(0, users.Length);

            Assert.Throws<UserExistsException>(() => userRepository.Save(new User(7, "User7", "Login7", users[randomIndex].Phone, "Mail7", "Password7")));

        }

        [Test]
        public void TestUserAddingExceptionThree()
        {
            int randomIndex = random.Next(0, users.Length);

            Assert.Throws<UserExistsException>(() => userRepository.Save(new User(8, "User8", "Login8", "Phone8", users[randomIndex].Mail, "Password8")));

        }

        [Test]
        public void TestAddingReview() {
            int randomCar = random.Next(0, cars.Length);
            List<Car> readCars = carRepository.FindAll();

            int randomUser = random.Next(0, users.Length);
            ushort randomRate = (ushort)random.Next(0, 6);

            Car oldReadCar = readCars.Find(x => x.Id == cars[randomCar].Id);
            uint oldRateNumber = oldReadCar.NumberOfRates;

            List<CarReview> carReviews = reviews.ToList().FindAll(r => r.CarId == cars[randomCar].Id);
            carReviews.Add(new CarReview(reviews.Length + 1, (long)cars[randomCar].Id, (long)users[randomUser].Id, randomRate, "Any Text"));

            reviewRepository.Save(carReviews.Last());
            List<Car> newReadCars = carRepository.FindAll();
            Car searchedNewCar = newReadCars.Find(c => c.Id == cars[randomCar].Id);

            List<CarReview> readReviews = reviewRepository.ReadByCar((long)searchedNewCar.Id);
            float countedMark = (float)readReviews.Sum(r => r.Rate) / readReviews.Count;
            Assert.That(searchedNewCar.NumberOfRates - 1, Is.EqualTo(oldRateNumber));
            Assert.That(searchedNewCar.Rate, Is.EqualTo(countedMark));
        }

        [Test]
        public void TestReadAllRents() {
            List<Rent> readRents = rentRepository.FindAll();
            for (int i = 0; i < rents.Length; i++) {
                Assert.IsTrue(AreTheSame(readRents[i], rents[i]));
            }
        }

        [Test]
        public void TestReadOneRent() {
            int randomRent = random.Next(0, rents.Length);
            Rent searchedRent = rentRepository.FindById((long)rents[randomRent].Id);
            Assert.IsTrue(AreTheSame(searchedRent, rents[randomRent]));
        }

        [Test]
        public void TestTakingRentsByCar() {
            int randomCarId = random.Next(0, cars.Length);
            List<Rent> rentsByCar = rents.ToList().FindAll(r => r.CarId == randomCarId);
            rentsByCar.Sort();
            List<Rent> rentsByCarRead = rentRepository.ReadForCar(randomCarId);
            rentsByCarRead.Sort();
            for (int i = 0; i < rentsByCarRead.Count; i++) {
                Assert.IsTrue(AreTheSame(rentsByCar[i], rentsByCarRead[i]));
            }
        }

        [Test]
        public void TestTakingRentsBetweenDates() {
            DateTime leftDateTime = new DateTime(2025, 4, 1, 1, 0, 0);
            DateTime rightDateTime = new DateTime(2025, 4, 30, 23, 59, 59);
            int randomCarId = random.Next(0, cars.Length);
            List<Rent> readRents = rentRepository.ReadForCarBetweenDates(randomCarId, leftDateTime, rightDateTime);
            List<Rent> selected = rents.ToList().FindAll(r => r.CarId == randomCarId && (r.StartRent > leftDateTime || r.EndRent < rightDateTime));
            for (int i = 0; i < readRents.Count; i++) {
                Assert.IsTrue(AreTheSame(readRents[i], selected[i]));
            }
        }

        [Test]
        public void TestTakingRentsByUser() {
            int randomUser = random.Next(0, users.Length);
            List<Rent> selectedRents = rents.ToList().FindAll(r => r.UserId == users[randomUser].Id);
            selectedRents.Sort();
            List<Rent> readRents = rentRepository.ReadForUser((long)users[randomUser].Id);
            readRents.Sort();
            for (int i = 0; i < readRents.Count; i++) {
                Assert.IsTrue(AreTheSame(readRents[i], selectedRents[i]));
            }
        }

        [Test]
        public void TestAddingRentException() {
            int randomUser = random.Next(0, users.Length);
            Assert.Throws<CarIsBuisyException>(
                () => rentRepository.Save(new Rent(rents.Length + 1, cars[4], users[randomUser], new DateTime(2025, 4, 22, 10, 15, 22), new DateTime(2025, 4, 27, 15, 20, 0))));
        }

        [TearDown]
        public void TearDown() { 
            carRepository.ClearAll();
            rentRepository.ClearAll();
            reviewRepository.ClearAll();
            userRepository.ClearAll();
        }

        private bool AreTheSame(Car carOne, Car carTwo) {
            return carOne.Id == carTwo.Id
                && carOne.Model.Mark == carTwo.Model.Mark
                && carOne.Model.Name.Equals(carTwo.Model.Name)
                && carOne.Model.Body == carTwo.Model.Body
                && carOne.Price == carTwo.Price;
        }

        private bool AreTheSame(User userOne, User userTwo) { 
            return userOne.Name.Equals(userTwo.Name) && userOne.Id == userTwo.Id && userOne.Mail.Equals(userTwo.Mail) && userOne.Login.Equals(userTwo.Login) &&
                userOne.Password.Equals(userTwo.Password) && userOne.Password.Equals(userTwo.Password);
        }

        private bool AreTheSame(Rent rentOne, Rent rentTwo) {
            return rentOne.StartRent.Equals(rentTwo.StartRent) && rentOne.EndRent.Equals(rentTwo.EndRent) && rentOne.Id == rentTwo.Id && rentOne.UserId == rentTwo.UserId;
        }

        private bool AreTheSame(CarReview reviewOne, CarReview reviewTwo) { 
            return reviewOne.Rate == reviewTwo.Rate && reviewOne.Id == reviewTwo.Id && reviewOne.UserId == reviewTwo.UserId && reviewOne.CarId == reviewTwo.CarId 
                && reviewOne.Text.Equals(reviewTwo.Text);
        }

        private ICarRepository carRepository;
        private IRentRepository rentRepository;
        private IReviewRepository reviewRepository;
        private IUserRepository userRepository;
    }
}