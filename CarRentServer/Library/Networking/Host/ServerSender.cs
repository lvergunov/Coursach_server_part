using Library.Networking.Controller;
using NetworkCommonEntities.Entities;

namespace Library.Networking.Host
{
    public class ServerSender
    {
        public ServerSender(ServerComponent serverComponent) { 
            _serverComponent = serverComponent;
        }

        public void SendTCP(ConnectedClient client, BytePackage bytePackage) {
            bytePackage.WriteLength();
            client.Tcp.SendBytes(bytePackage);
        }

        public void SendMultiples(ConnectedClient client, QueryResultFlag queryResultFlag, string entity, string listJsonOrError) {
            using (BytePackage _package = new BytePackage((int)ServerPackets.sendListOfEntities)) {
                _package.Write((int)queryResultFlag);
                _package.Write(entity);
                _package.Write(listJsonOrError);
                SendTCP(client, _package);
            }
        }

        public void SendSingle(ConnectedClient client, QueryResultFlag resultFlag, string entityName, string entityJsonOrError) {
            using (BytePackage _package = new BytePackage((int)ServerPackets.sendOneEntity)) {
                _package.Write((int)resultFlag);
                _package.Write(entityName);
                _package.Write(entityJsonOrError);
                SendTCP(client, _package);
            }
        }

        public void WelcomeClient(ConnectedClient connectedClient) {
            using (BytePackage _package = new BytePackage((int)ServerPackets.welcome)) {
                _package.Write(connectedClient.Id);
                SendTCP(connectedClient, _package);
            }
        }

        public void SendRegistrationResult(string message, QueryResultFlag queryResult, ConnectedClient cc) {
            using (BytePackage package = new BytePackage((int)ServerPackets.userIsRegistrated)) {
                package.Write((int)queryResult);
                package.Write(message);
                SendTCP(cc, package);
            }
        }

        public void SendVerificationResult(ConnectedClient cc, QueryResultFlag queryResult, string message) {
            using (BytePackage package = new BytePackage((int)ServerPackets.userVerified)) {
                package.Write((int)queryResult);
                package.Write(message);
                SendTCP(cc, package);
            }
        }

        public void SendChangingPasswordResult(ConnectedClient cc, QueryResultFlag queryResult, string message) {
            using (BytePackage package = new BytePackage((int)ServerPackets.confirmChangingPassword)) {
                package.Write((int)queryResult);
                package.Write(message);
                SendTCP(cc, package);
            }
        }

        public void DisconnectClient(ConnectedClient connectedClient) {
            using (BytePackage package = new BytePackage((int)ServerPackets.disconnectClient)) {
                SendTCP(connectedClient, package);
            }
        }

        public void DisconnectHost(Dictionary<int, ConnectedClient> clients) {
            using (BytePackage package = new BytePackage((int)ServerPackets.disonnectHost)) {
                package.Write(0);
                BroadcastMessage(clients, package);
            }
        }

        public void SendCrudResult(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.confirmCRUD)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendSaveResult(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.confirmSaving))
            {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendFiltratedCars(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.carsFiltration)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendAllCarsForRent(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.allCarsForRent)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendAllUsersForRent(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.allUsersForRent)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendAllCarsRentedByUser(ConnectedClient client, QueryResultFlag queryResult, string result)
        {
            using (BytePackage package = new BytePackage((int)ServerPackets.rentedCarsByUser)) { 
                package.Write ((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendAllRentsByUser(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.rentsByUser)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendAllReviewsByUser(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.reviewsByUser)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendAllReviewsByCar(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.reviewsByCar)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void SendReviewAuthor(ConnectedClient client, QueryResultFlag queryResult, string result) {
            using (BytePackage package = new BytePackage((int)ServerPackets.reviewAuthor)) {
                package.Write((int)queryResult);
                package.Write(result);
                SendTCP(client, package);
            }
        }

        public void BroadcastMessage(Dictionary<int, ConnectedClient> clients, BytePackage package) {
            package.WriteLength();
            foreach (var cl in clients.Values) {
                if (cl.Tcp.Active) {
                    cl.Tcp.SendBytes(package);
                }
            }
        }
        private ServerComponent _serverComponent;
    }
}
