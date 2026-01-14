using NetworkCommonEntities.Entities;

namespace Library.Networking.Host
{
    public class ServerHandler
    {
        public ServerHandler(ServerComponent serverComponent) { 
            _serverComponent = serverComponent;
        }

        public void ReceiveOnWelcome(int fromClient, BytePackage bytePackage) { 
            int clientId = bytePackage.ReadByte();
            if (fromClient == clientId)
            {
                _serverComponent.WriteConnectionInfo($"Connected user with id: {fromClient}");
            }
            else {
                _serverComponent.WriteConnectionInfo($"Error in identification user {fromClient}");
                _serverComponent.Disconnect(false);
            }
        }

        public void ReceiveRegistration(int fromClient, BytePackage bytePackage) { 
            string serializedUser = bytePackage.ReadString();
            _serverComponent.RegistrateNewUser(fromClient, serializedUser);
        }

        public void ReceiveVerificationRequest(int fromClient, BytePackage bytePackage) { 
            string login = bytePackage.ReadString();
            string password = bytePackage.ReadString();
            _serverComponent.VerifyUser(fromClient, login, password);
        }

        public void ReceiveOnDisconnectUser(int fromClientId, BytePackage bytePackage) {
            int clientId = bytePackage.ReadInt();
            _serverComponent.DisconnectOne(fromClientId);
        }

        public void ReceiveRequestForMultiples(int fromClientId, BytePackage bytePackage) { 
            string entityName = bytePackage.ReadString();
            _serverComponent.ReadAllMultiples(fromClientId, entityName);
        }

        public void ReceiveRequestById (int fromClientId, BytePackage bytePackage) {
            string entityName = bytePackage.ReadString();
            long id = bytePackage.ReadLong();
            _serverComponent.GetEntityInfoById(fromClientId, entityName, id);
        }

        public void ChangeUserPassword(int fromClientId, BytePackage bytePackage) { 
            long id = bytePackage.ReadLong();
            string newPassword = bytePackage.ReadString();
            _serverComponent.UpdateUserPassword(fromClientId, id, newPassword);
        }

        public void ReceiveDelete(int fromClientId, BytePackage bytePackage) {
            string entityName = bytePackage.ReadString();
            long entityId = bytePackage.ReadLong();
            _serverComponent.DeleteEntityById(fromClientId, entityName, entityId);
        }

        public void ReceiveSaveEntity(int fromClientId, BytePackage bytePackage) { 
            string entityName = bytePackage.ReadString();
            string entityJson = bytePackage.ReadString();
            _serverComponent.SaveNewEntity(fromClientId, entityName, entityJson);
        }

        public void ReceiveCarManufacturersRequest(int fromClientId, BytePackage bytePackage) {
            _serverComponent.ReadAllMultiples(fromClientId, "manufacturer");
        }

        public void ReceiveCarBodiesRequest(int fromClientId, BytePackage bytePackage) {
            _serverComponent.ReadAllMultiples(fromClientId, "body");
        }

        public void ReceiveFiltratedCars(int fromClientId, BytePackage bytePackage) { 
            string serializedFilter = bytePackage.ReadString();
            _serverComponent.FiltrateCars(fromClientId, serializedFilter);
        }

        public void ReceiveAllCarsForRent(int fromClientId, BytePackage bytePackage) {
            _serverComponent.GetAllCarsForRent(fromClientId);
        }

        public void ReceiveAllUsersForRent(int fromClientId, BytePackage bytePackage) {
            _serverComponent.GetAllUsersForRent(fromClientId);
        }

        public void ReceiveAllCarsRentedByUser(int fromClientId, BytePackage bytePackage) { 
            long userId = bytePackage.ReadLong();
            _serverComponent.SendRentedByUserCars(fromClientId, userId);
        }

        public void ReceiveAllRentsByUser(int fromClientId, BytePackage bytePackage) { 
            long userId = bytePackage.ReadLong();
            _serverComponent.SendAllRentsByUser(fromClientId, userId);
        }

        public void ReceiveReviewsByUser(int fromClientId, BytePackage bytePackage) {
            long userId = bytePackage.ReadLong();
            _serverComponent.SendReviewByUser(fromClientId, userId);
        }

        public void ReceiveReviewsByCar(int fromClientId, BytePackage bytePackage) { 
            long carId = bytePackage.ReadLong();
            _serverComponent.SendReviewsByCar(fromClientId, carId);
        }

        public void ReceiveReviewAuthor(int fromClientId, BytePackage bytePackage) { 
            long userId = bytePackage.ReadLong();
            _serverComponent.SendReviewAuthor(fromClientId, userId);
        }

        private ServerComponent _serverComponent;
    }
}
