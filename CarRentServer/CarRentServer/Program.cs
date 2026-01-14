using Library.Json.Implementation.Reader;
using Library.Json.Reader;
using Library.Networking.Host;
using NetworkCommonEntities.Entities;

namespace CarRentServer { 

internal class Program {

        static void Main(string[] args)
        {
            Console.WriteLine("Opening server started.");
            Console.WriteLine("Enter port number");
            try
            {
                int portNumber = Int32.Parse(Console.ReadLine());
                serverComponent = new ServerComponent(new Threading(), portNumber);
                serverComponent.onError += OutMessage;
                serverComponent.onUserConnection += OutMessage;
                serverComponent.onUserDisconnection += OutUserConnection;
                serverComponent.onUserVerification += OutUserVerification;
                serverComponent.StartConnectionListener();
                Console.WriteLine("Server successfully opened!");
                Console.WriteLine("Press Esc to stop all connections");
                Thread keyListener = new Thread(WaitForExitKey);
                keyListener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void OutMessage(string text)
        {
            Console.WriteLine(text);
        }

        public static void OutUserConnection(int clientId)
        {
            Console.WriteLine($"Client {clientId} is disconnected.");
        }

        public static void OutUserVerification(int clientId, string login) {
            Console.WriteLine($"Client {clientId} is verified like user {login}");
        }

        public static void WaitForExitKey()
        {
            while (_isRunning)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape) { 
                        _isRunning = false;
                        serverComponent.Disconnect(true);
                        break;
                    }
                }
            }
        }
        private static ServerComponent serverComponent;

        private static bool _isRunning = true;
    }       
}


