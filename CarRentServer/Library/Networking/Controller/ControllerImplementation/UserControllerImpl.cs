using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Repository;
using System.Text.Json;

namespace Library.Networking.Controller.ControllerImplementation
{
    public class UserControllerImpl : IUserController
    {

        public UserControllerImpl(IUserRepository repository) { 
            _userRepository = repository;
        }

        public string ChangeUsersPassword(long userId, string password, out QueryResultFlag queryResult)
        {
            try {
                _userRepository.ChangeUserPassword(userId, password);
                queryResult = QueryResultFlag.Success;
                return "Password successfully changed!";
            } catch (WriteFileException ex) { 
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
        }

        public string Delete(long id, out QueryResultFlag queryResult)
        {
            try {
                _userRepository.RemoveById(id);
                queryResult = QueryResultFlag.Success;
                return "Account is successfully deleted.";
            } catch (WriteFileException ex) { 
                queryResult= QueryResultFlag.Error;
                return ex.Message;
            }
        }

        public string GetAll(out QueryResultFlag queryResult)
        {
            try {
                List<User> users = _userRepository.FindAll();
                string serializedString = JsonSerializer.Serialize(new UserList() { Users = users });
                queryResult = QueryResultFlag.Success;
                return serializedString;
            } catch (ReadFileException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            } catch (NotSupportedException ex) {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetById(long id, out QueryResultFlag queryResult)
        {
            try
            {
                var searchedUser = _userRepository.FindById(id);
                string serializedString = JsonSerializer.Serialize(searchedUser);
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException ex) {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetFromTo(long leftId, long rightId, out QueryResultFlag queryResult)
        {
            try
            {
                List<User> users = _userRepository.FindAll();
                string serializedString = JsonSerializer.Serialize(new UserList() { Users = users });
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetUserByLogin(string login, out QueryResultFlag queryResult)
        {
            try
            {
                var searchedUser = _userRepository.ReadUserByLogin(login);
                string serializedString = JsonSerializer.Serialize(searchedUser);
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException ex) {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetUserByMail(string mail, out QueryResultFlag queryResult)
        {
            try
            {
                var searchedUser = _userRepository.ReadUserByEmail(mail);
                string serializedString = JsonSerializer.Serialize(searchedUser);
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string GetUserByPhone(string phone, out QueryResultFlag queryResult)
        {
            try
            {
                var searchedUser = _userRepository.ReadUserByPhone(phone);
                string serializedString = JsonSerializer.Serialize(searchedUser);
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string Save(string jsonQuery, out QueryResultFlag queryResult)
        {
            try
            {
                User desUser = JsonSerializer.Deserialize<User>(jsonQuery);
                if (desUser == null)
                {
                    queryResult = QueryResultFlag.Error;
                    return "Serialization error.";
                }
                _userRepository.Save(desUser);
                queryResult = QueryResultFlag.Success;
                return "Added succsessfully!";
            }
            catch (UserExistsException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (WriteFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (ArgumentNullException ex) {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (JsonException ex) {
                queryResult = QueryResultFlag.Error;
                return "Error in file structure!";
            } catch (NotSupportedException ex) { 
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        public string VerifyUser(string login, string password, out QueryResultFlag queryResult)
        {
            try
            {
                var searchedUser = _userRepository.VerifyUser(login, password);
                string serializedString = JsonSerializer.Serialize(searchedUser);
                queryResult = QueryResultFlag.Success;
                return serializedString;
            }
            catch (ReadFileException ex)
            {
                queryResult = QueryResultFlag.Error;
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                queryResult = QueryResultFlag.Error;
                return "Logic functional error!";
            }
        }

        private IUserRepository _userRepository;
    }
}
