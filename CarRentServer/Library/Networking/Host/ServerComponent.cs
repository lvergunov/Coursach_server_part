using Library.Entities;
using Library.Networking.Controller;
using Library.Networking.Controller.ControllerImplementation;
using Library.Repository.Implementation;
using NetworkCommonEntities.Entities;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Library.Networking.Host
{
    public delegate void ServerPackageHandler(int fromClientId, BytePackage package);
    public delegate void UserConnectionHandler(string message);
    public delegate void UserInteractionHandler(int id);
    public delegate void UserVerificationHandler(int clientId, string login);

    public class ServerComponent : NetworkHandler
    {
        public event UserConnectionHandler onUserConnection;
        public event UserConnectionHandler onError;
        public event UserInteractionHandler onUserDisconnection;
        public event UserVerificationHandler onUserVerification;

        private const int MAX_USERS_AMOUNT = 64;
        public ServerSender Sender { get { return _serverSender; } }
        public ServerHandler Handler { get { return _serverHandler; } }

        public static ServerComponent Instance { get; private set; }

        public Dictionary<int, ServerPackageHandler> ActionsForPackage { get; private set; }
        public ServerComponent(Threading threadManager, int portNumber) : base(threadManager, portNumber)
        {
            Instance = this;
            _listener = new TcpListener(IPAddress.IPv6Any, portNumber);
            _listener.Server.DualMode = true;
            _serverSender = new ServerSender(this);
            _serverHandler = new ServerHandler(this);
            CarRepositoryImpl carRepository = new CarRepositoryImpl();
            RentRepositoryImpl rentRepository = new RentRepositoryImpl();
            ReviewRepositoryImpl reviewRepository = new ReviewRepositoryImpl();
            UserRepository userRepository = new UserRepository();
            ManufacturerRepository manufacturerRepository = new ManufacturerRepository();

            _carController = new CarControllerImpl(carRepository, rentRepository, manufacturerRepository);
            _rentController = new RentControllerImpl(rentRepository);
            _reviewController = new ReviewControllerImpl(reviewRepository);
            _userController = new UserControllerImpl(userRepository);

            for (int i = 1; i < MAX_USERS_AMOUNT; i++) {
                _connectedClients.Add(i, new ConnectedClient(i, this, ThreadManager));
            }

            ActionsForPackage = new Dictionary<int, ServerPackageHandler>()
            {
                { (int)ClientPackets.welcomeReceived, _serverHandler.ReceiveOnWelcome },
                { (int)ClientPackets.disconnect, _serverHandler.ReceiveOnDisconnectUser },
                { (int)ClientPackets.userRegistration, _serverHandler.ReceiveRegistration },
                { (int)ClientPackets.userVerification, _serverHandler.ReceiveVerificationRequest },
                { (int)ClientPackets.getAll, _serverHandler.ReceiveRequestForMultiples },
                { (int)ClientPackets.getById, _serverHandler.ReceiveRequestById },
                { (int)ClientPackets.changeUserPassword, _serverHandler.ChangeUserPassword },
                { (int)ClientPackets.deleteEntity, _serverHandler.ReceiveDelete },
                { (int)ClientPackets.getCarManufacturers, _serverHandler.ReceiveCarManufacturersRequest },
                { (int)ClientPackets.getCarBodies, _serverHandler.ReceiveCarBodiesRequest },
                { (int)ClientPackets.saveEntity, _serverHandler.ReceiveSaveEntity },
                { (int)ClientPackets.getCarsByFiltration, _serverHandler.ReceiveFiltratedCars },
                { (int)ClientPackets.getAllCarsForRent, _serverHandler.ReceiveAllCarsForRent },
                { (int)ClientPackets.getAllUsersForRent, _serverHandler.ReceiveAllUsersForRent },
                { (int)ClientPackets.getRentedCarsByUser, _serverHandler.ReceiveAllCarsRentedByUser },
                { (int)ClientPackets.getAllRentsByUser, _serverHandler.ReceiveAllRentsByUser },
                { (int)ClientPackets.getAllReviewsByUser, _serverHandler.ReceiveReviewsByUser },
                { (int)ClientPackets.getAllReviewsByCar, _serverHandler.ReceiveReviewsByCar },
                { (int)ClientPackets.getReviewAuthor, _serverHandler.ReceiveReviewAuthor }
            };
        }

        public void SendReviewAuthor(int clientId, long userId) {
            string result = _userController.GetById(userId, out QueryResultFlag queryResult);
            _serverSender.SendReviewAuthor(_connectedClients[clientId], queryResult, result);
        }

        public void SendReviewsByCar(int clientId, long carId) { 
            string result = _reviewController.ReadByCar(carId, out QueryResultFlag queryResult);
            _serverSender.SendAllReviewsByCar(_connectedClients[clientId], queryResult, result);
        }

        public void SendReviewByUser(int clientId, long userId) { 
            string result = _reviewController.ReadByUser(userId, out QueryResultFlag queryResult);
            _serverSender.SendAllReviewsByUser(_connectedClients[clientId], queryResult, result);
        }

        public void SendRentedByUserCars(int clientId, long userId) {
            string result = _carController.GetAllByUserRents(userId, out QueryResultFlag queryResult);
            _serverSender.SendAllCarsRentedByUser(_connectedClients[clientId], queryResult, result);
        }

        public void SendAllRentsByUser(int clientId, long userId) {
            string result = _rentController.GetForUser(userId, out QueryResultFlag queryResult);
            _serverSender.SendAllRentsByUser(_connectedClients[clientId], queryResult, result);
        }

        public void FiltrateCars(int clientId, string serializedFilter) { 
            FilterJson filterJson = JsonSerializer.Deserialize<FilterJson>(serializedFilter);
            string carList = _carController.GetCarsByFiltration(serializedFilter, out QueryResultFlag queryResult);
            _serverSender.SendFiltratedCars(_connectedClients[clientId], queryResult, carList);
        }

        public void GetAllCarsForRent(int clientId) {
            string allCars = _carController.GetAll(out QueryResultFlag queryResult);
            _serverSender.SendAllCarsForRent(_connectedClients[clientId], queryResult, allCars);
        }

        public void GetAllUsersForRent(int clientId) { 
            string allUsers = _userController.GetAll(out QueryResultFlag queryResult);
            _serverSender.SendAllUsersForRent(_connectedClients[clientId], queryResult, allUsers);
        }

        public void UpdateUserPassword(int fromClientId, long id, string password) {
            string result = _userController.ChangeUsersPassword(id, password, out QueryResultFlag queryResult);
            _serverSender.SendChangingPasswordResult(_connectedClients[fromClientId], queryResult, result);
        }

        public void GetEntityInfoById(int fromClientId, string entityName, long id) {
            switch (entityName) {
                case "user":
                    string result = _userController.GetById(id, out QueryResultFlag resultFlag);
                    _serverSender.SendSingle(_connectedClients[fromClientId], resultFlag, entityName, result);
                    break;
                case "car":
                    string carResult = _carController.GetById(id, out resultFlag);
                    _serverSender.SendSingle(_connectedClients[fromClientId], resultFlag, entityName, carResult);
                    break;
            }
        }

        public void DeleteEntityById(int fromClientId, string entityName, long id) {
            switch (entityName) {
                case "user":
                    string result = _userController.Delete(id, out QueryResultFlag queryResult);
                    _serverSender.SendCrudResult(_connectedClients[fromClientId], queryResult, result);
                    break;
                case "car":
                    result = _carController.Delete(id, out queryResult);
                    _serverSender.SendSaveResult(_connectedClients[fromClientId], queryResult, result);
                    break;
            }
        }

        public void VerifyUser(int fromClientId, string login, string password) {
            onUserVerification(fromClientId, login);
            string serializedUser = _userController.VerifyUser(login, password, out QueryResultFlag queryResult);
            _serverSender.SendVerificationResult(_connectedClients[fromClientId], queryResult, serializedUser);
        }

        public void RegistrateNewUser(int fromClientId, string userInfo) {
            string message = _userController.Save(userInfo, out QueryResultFlag queryResult);
            _serverSender.SendRegistrationResult(message, queryResult, _connectedClients[fromClientId]);
        }

        public void WriteConnectionInfo(string message) {
            onUserConnection(message);
        }

        public override void Disconnect(bool withUniform)
        {
            try
            {
                _newForListener = null;
                foreach (var cl in _connectedClients.Values)
                {
                    if (cl.Tcp.Client != null && cl.Tcp.Client.Connected)
                    {
                        _serverSender.DisconnectHost(_connectedClients);
                        cl.Disconnect();
                    }
                }
            }
            catch
            {
                onError("Error in user disconnection!");
            }
            finally { 
                _listener.Stop();
            }
        }

        public void DisconnectOne(int id) {
            try
            {
                onUserDisconnection(id);
                _serverSender.DisconnectClient(_connectedClients[id]);
                _connectedClients[id].Disconnect();
            }
            catch {
                onError("Error in user disconnection!");
            }
        }

        public void ReadAllMultiples(int fromClientId, string entityName) {
            switch (entityName) {
                case "user":
                    string result = _userController.GetAll(out QueryResultFlag resultFlag);
                    _serverSender.SendMultiples(_connectedClients[fromClientId], resultFlag, entityName, result);
                    break;
                case "car":
                    string resultCars = _carController.GetAll(out QueryResultFlag resultFlagCars);
                    _serverSender.SendMultiples(_connectedClients[fromClientId], resultFlagCars, entityName, resultCars);
                    break;
                case "manufacturer":
                    string resultManufacturers = _carController.GetCarManufacturers(out QueryResultFlag manufacturerResultFlag);
                    _serverSender.SendMultiples(_connectedClients[fromClientId], manufacturerResultFlag, entityName, resultManufacturers);
                    break;
                case "body":
                    string resultBodies = _carController.GetCarBodies(out QueryResultFlag bodyResultFlag);
                    _serverSender.SendMultiples(_connectedClients[fromClientId], bodyResultFlag, entityName, resultBodies);
                    break;
                case "rent":
                    string resultRents = _rentController.GetAll(out QueryResultFlag rentResultFlag);
                    _serverSender.SendMultiples(_connectedClients[fromClientId], rentResultFlag, entityName, resultRents);
                    break;
                case "review":
                    string resultReviews = _rentController.GetAll(out QueryResultFlag reviewResultFlag);
                    _serverSender.SendMultiples(_connectedClients[fromClientId], reviewResultFlag, entityName, resultReviews);
                    break;
            }
        }

        public void SaveNewEntity(int fromClientId, string entityName, string entityJson) {
            switch (entityName) { 
                case "car":
                    string savingResult = _carController.Save(entityJson, out QueryResultFlag queryResult);
                    _serverSender.SendSaveResult(_connectedClients[fromClientId], queryResult, savingResult);
                    break;
                case "rent":
                    string savingResultRent = _rentController.Save(entityJson, out QueryResultFlag queryResultRent);
                    _serverSender.SendSaveResult(_connectedClients[fromClientId], queryResultRent, savingResultRent);
                    break;
                case "review":
                    string savingResultReview = _reviewController.Save(entityJson, out QueryResultFlag queryResultReview);
                    _serverSender.SendSaveResult(_connectedClients[fromClientId], queryResultReview, savingResultReview);
                    break;

            }
        }

        public void StartConnectionListener() {
            _threadUpdater = new ThreadUpdater(ThreadManager);
            _newForListener = new Thread(_threadUpdater.UpdateThread);
            _newForListener.Start();
            _listener.Start();
            _listener.BeginAcceptTcpClient(new AsyncCallback(WorkWithNewClient), null);
        }

        private void WorkWithNewClient(IAsyncResult _result) {
            try
            {
                TcpClient tcpClient = _listener.EndAcceptTcpClient(_result);
                _listener.BeginAcceptTcpClient(new AsyncCallback(WorkWithNewClient), null);
                for (int i = 1; i <= MAX_USERS_AMOUNT; i++)
                {
                    if (_connectedClients[i].Tcp.Client == null)
                    {
                        _connectedClients[i].Tcp.Connect(tcpClient);
                        _serverSender.WelcomeClient(_connectedClients[i]);
                        return;
                    }
                }
            }
            catch (System.Exception ex) {
                onError(ex.Message);
            }
        }

        private Thread _newForListener;
        private TcpListener _listener;
        private ServerHandler _serverHandler;
        private ServerSender _serverSender;
        private ThreadUpdater _threadUpdater;
        private Dictionary<int, ConnectedClient> _connectedClients = new Dictionary<int, ConnectedClient>();
        private IUserController _userController;
        private ICarController _carController;
        private IRentController _rentController;
        private IReviewController _reviewController;
    }
}
