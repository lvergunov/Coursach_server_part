using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Repository;
using System.Text.Json;

namespace Library.Networking.Controller.ControllerImplementation
{
    public class RentControllerImpl : IRentController
    {
        public RentControllerImpl(IRentRepository rentRepository) { 
            _rentRepository = rentRepository;
        }
      
        public string Delete(long id, out QueryResultFlag queryResult)
        {
            try { 
                _rentRepository.RemoveById(id);
                queryResult = new QueryResultFlag();
                return "Succsessfully deleted!";
            }
            catch(WriteFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
        }

        public string GetAll(out QueryResultFlag queryResult)
        {
            try {
                List<Rent> rents = _rentRepository.FindAll();
                string serializedList = JsonSerializer.Serialize(new RentList() { Rents = rents });
                queryResult = QueryResultFlag.Success;
                return serializedList;
            }
            catch (ReadFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException) { 
                queryResult= QueryResultFlag.Error;
                return "Functional logic error!";
            }
        }

        public string GetById(long id, out QueryResultFlag queryResult)
        {
            try
            {
                Rent rent = _rentRepository.FindById(id);
                string serialized = JsonSerializer.Serialize(rent);
                queryResult = QueryResultFlag.Success;
                return serialized;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException) {
                queryResult = QueryResultFlag.Error;
                return "Functional logic error!";
            }
        }

        public string GetForCarBetweenDates(long carId, DateTime leftDate, DateTime rightDate, QueryResultFlag queryResult)
        {
            try {
                List<Rent> rents = _rentRepository.ReadForCarBetweenDates(carId, leftDate, rightDate);
                string serializedRents = JsonSerializer.Serialize(new RentList() { Rents = rents });
                queryResult = QueryResultFlag.Success;
                return serializedRents;
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

        public string GetForUser(long userId, out QueryResultFlag queryResult)
        {
            try {
                List<Rent> rents = _rentRepository.ReadForUser(userId);
                string serializedRents = JsonSerializer.Serialize(new RentList() { Rents = rents });
                queryResult = QueryResultFlag.Success;
                return serializedRents;
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

        public string GetForUserBetweenDates(long userId, DateTime startDate, DateTime endDate, out QueryResultFlag queryResult)
        {
            try
            {
                List<Rent> rents = _rentRepository.ReadForUserBetweenDates(userId, startDate, endDate);
                string serializedRents = JsonSerializer.Serialize(new RentList() { Rents = rents });
                queryResult = QueryResultFlag.Success;
                return serializedRents;
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

        public string GetFromTo(long leftId, long rightId, out QueryResultFlag queryResult)
        {
            try
            {
                List<Rent> rents = _rentRepository.FindFromTo(leftId, rightId);
                string serializedRents = JsonSerializer.Serialize(new RentList() { Rents = rents });
                queryResult = QueryResultFlag.Success;
                return serializedRents;
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

        public string GetRentsForCar(long carId, QueryResultFlag queryResult)
        {
            try
            {
                List<Rent> rents = _rentRepository.ReadForCar(carId);
                string serializedRents = JsonSerializer.Serialize(new RentList() { Rents = rents });
                queryResult = QueryResultFlag.Success;
                return serializedRents;
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
                Rent deserealisedRent = JsonSerializer.Deserialize<Rent>(jsonQuery);
                if (deserealisedRent == null) {
                    queryResult = QueryResultFlag.Error;
                    return "Json format error!";
                }
                _rentRepository.Save(deserealisedRent);
                queryResult = QueryResultFlag.Success;
                return "Added successfully!";
            }
            catch (CarIsBuisyException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
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

        private IRentRepository _rentRepository;
    }
}
