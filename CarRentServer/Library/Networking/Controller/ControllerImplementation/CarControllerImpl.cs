using CarRentEntities.Entities.Collections;
using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Repository;
using System.Text.Json;

namespace Library.Networking.Controller.ControllerImplementation
{
    public class CarControllerImpl : ICarController
    {
        public CarControllerImpl(ICarRepository carRepository, IRentRepository rentRepository, 
            IManufacturerRepository manufacturerRepository) { 
            _carRepository = carRepository;
            _rentRepository = rentRepository;
            _manufacturerRepository = manufacturerRepository;
        }
        public string Delete(long id, out QueryResultFlag queryResult)
        {
            try
            {
                _carRepository.RemoveById(id);
                queryResult = QueryResultFlag.Success;
                return "Successfully deleted!";
            }
            catch (WriteFileException ex){
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
        }

        public string GetAll(out QueryResultFlag queryResult)
        {
            try {
                List<Car> cars = _carRepository.FindAll();
                string serializedCars = JsonSerializer.Serialize(new CarList() { Cars = cars });
                queryResult = QueryResultFlag.Success;
                return serializedCars;
            }
            catch (ReadFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException) {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetById(long id, out QueryResultFlag queryResult)
        {
            try { 
                Car car = _carRepository.FindById(id);
                string serializedCar = JsonSerializer.Serialize(car);
                queryResult = QueryResultFlag.Success;
                return serializedCar;
            }
            catch (ReadFileException ex) { 
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException)
            {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetCarBodies(out QueryResultFlag queryResult)
        {
            try
            {
                string serializedString = JsonSerializer.Serialize(new CarBodyList() { 
                    CarBodies = _manufacturerRepository.GetAllBodies() 
                });
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (NotSupportedException) { 
                queryResult= QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetCarManufacturers(out QueryResultFlag queryResult)
        {
            try
            {
                string serializedString = JsonSerializer.Serialize(new CarManufacturerList() {
                    ManufacturerMarks = _manufacturerRepository.GetAllManufacturers()
                });
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (NotSupportedException) {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetCarsByFiltration(string jsonParams, out QueryResultFlag queryResult)
        {
            try
            {
                FilterJson serializedParams = JsonSerializer.Deserialize<FilterJson>(jsonParams);
                if (serializedParams == null)
                {
                    queryResult = QueryResultFlag.Error;
                    return "File format error!";
                }
                List<Car> searchedCars = _carRepository.Filter(serializedParams.LowCost, serializedParams.HighCost, serializedParams.CarBody,
                    serializedParams.Manufacturer, serializedParams.Name, serializedParams.Active);
                if (serializedParams.StartDate != null && serializedParams.EndDate != null)
                {
                    searchedCars = ListFreeCars((DateTime)serializedParams.StartDate, (DateTime)serializedParams.EndDate).
                        Intersect(searchedCars).ToList();
                }
                string serializedList = JsonSerializer.Serialize(new CarList() { Cars = searchedCars });
                queryResult = QueryResultFlag.Success;
                return serializedList;
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
            catch (NotSupportedException) { 
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetFreeCars(DateTime leftDate, DateTime rightDate, out QueryResultFlag queryResult)
        {
            try {
                List<Car> freeCars = ListFreeCars(leftDate, rightDate);
                string serializedCars = JsonSerializer.Serialize(new CarList() { Cars = freeCars });
                queryResult = QueryResultFlag.Success;
                return serializedCars;
            } catch (ReadFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            } catch (NotSupportedException) {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetFromTo(long leftId, long rightId, out QueryResultFlag queryResult)
        {
            try {
                List<Car> searchedCars = _carRepository.FindFromTo(leftId, rightId);
                string serializedCars = JsonSerializer.Serialize(new CarList() { Cars = searchedCars });
                queryResult = QueryResultFlag.Success;
                return serializedCars;
            }
            catch (ReadFileException ex) { 
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
        }

        public string Save(string jsonQuery, out QueryResultFlag queryResult)
        {
            try { 
                Car carToSave = JsonSerializer.Deserialize<Car>(jsonQuery);
                if (carToSave == null) {
                    queryResult = QueryResultFlag.Error;
                    return "Json format error";
                }
                _carRepository.Save(carToSave);
                queryResult = QueryResultFlag.Success;
                return "Successfully added!";
            }
            catch (WriteFileException ex) { 
                queryResult= QueryResultFlag.Error;
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
            catch (NotSupportedException) { 
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetActiveNow(out QueryResultFlag queryResult)
        {
            try {
                List<Car> activeCars = _carRepository.FindCarByActivity(true);
                string serializedCars = JsonSerializer.Serialize(new CarList() { Cars = activeCars });
                queryResult = QueryResultFlag.Success;
                return serializedCars;
            }
            catch (ReadFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
        }

        private List<Car> ListFreeCars(DateTime leftDate, DateTime rightDate) {
            List<Car> buisyCars = new List<Car>();
            List<Car> allCars = _carRepository.FindAll();
            List<Rent> allRents = _rentRepository.FindAll();
            foreach (Rent r in allRents)
            {
                if (_rentRepository.AreDatesCrossed(r, leftDate, rightDate))
                {
                    buisyCars.Add(allCars.Find(c => c.Id == r.CarId));
                }
            }
            return allCars.Except(buisyCars).ToList();
        }

        public string GetAllByUserRents(long userId, out QueryResultFlag queryResult)
        {
            try
            {
                List<Rent> userRents = _rentRepository.FindAll();
                List<Car> rentedCars = new List<Car>();
                foreach (Rent r in userRents) {
                    if (rentedCars.Find(rc => rc.Id == r.CarId) == null)
                    {
                        Car car = _carRepository.FindById(r.CarId);
                        rentedCars.Add(car);
                    }
                }
                queryResult = QueryResultFlag.Success;
                return JsonSerializer.Serialize(new CarList() { Cars = rentedCars });
            }
            catch (ReadFileException ex) { 
                queryResult= QueryResultFlag.Error;
                return ex.Message;
            }
        }

        private ICarRepository _carRepository;
        private IRentRepository _rentRepository;
        private IManufacturerRepository _manufacturerRepository;
    }
}
