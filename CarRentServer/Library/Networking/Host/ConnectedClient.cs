using NetworkCommonEntities.Entities;
using System.Net.Sockets;

namespace Library.Networking.Host
{
    public class ConnectedClient
    {
        public TCP Tcp {
            get {
                return _tcp;
            }
        }

        public string IP {
            get {
                return _tcp.Client.Client.LocalEndPoint.ToString();
            }
        }

        public ConnectedClient(int id, ServerComponent serverComponent, Threading threadManager) {
            Id = id;
            _tcp = new TCP(id, serverComponent, threadManager);
        }

        public int Id { get; }

        public void Disconnect() { 
            _tcp.Disconnect();
        }

        public class TCP { 
            public bool Active {
                get {
                    return _active;
                }
            }

            public int bufferSize = 16 * 128;
            public TcpClient Client { get; private set; }
            public ServerComponent ServerComponent { get { return _serverRef; } }

            private readonly int id;
            private NetworkStream _networkStream;
            private Threading _threadManager;
            private byte[] _receiveBuffer;
            private ServerComponent _serverRef;
            private bool _active;
            private BytePackage _receivePackage;
            public TCP(int _id, ServerComponent serverRef, Threading threading) {
                id = _id;
                _serverRef = serverRef;
                _threadManager = threading;
                _receivePackage = new BytePackage();
            }

            public void Connect(TcpClient _client) { 
                Client = _client;
                Client.ReceiveBufferSize = bufferSize;
                Client.SendBufferSize = bufferSize;
                _receiveBuffer = new byte[bufferSize];
                _networkStream = Client.GetStream();
                _active = true;
                try
                {
                    _networkStream.BeginRead(_receiveBuffer, 0, bufferSize, ReceiveCallback, null);
                }
                catch (System.Exception ex) {
                    Disconnect();
                }
            }

            public void SendBytes(BytePackage bytePackage) {
                try {
                    _networkStream.BeginWrite(bytePackage.ToArray(), 0, bytePackage.Length, null, null);
                }
                catch (System.Exception ex) {
                    Disconnect();
                }
            }

            public void Disconnect() { 
                _active = false;
                Client.Close();
                _networkStream = null;
                _receiveBuffer = null;
                Client = null;
            }

            private void ReceiveCallback(IAsyncResult _result) { 
                try { 
                    if (_networkStream != null) { 
                        int _byteLength = _networkStream.EndRead(_result);
                        if (_byteLength <= 0)
                        {
                            Disconnect();
                            return;
                        }
                        byte[] _data = new byte[_byteLength];
                        Array.Copy(_receiveBuffer, _data, _byteLength);
                        _receivePackage.Reset(HandleData(_data));
                        _networkStream.BeginRead(_receiveBuffer, 0, bufferSize, ReceiveCallback, null);
                    }
                }
                catch { }
            }

            private bool HandleData(byte[] _data) {
                int _packetLength = 0;
                _receivePackage.SetBytes(_data);

                if (_receivePackage.UnreadLength >= 4) {
                    _packetLength = _receivePackage.ReadInt();
                    if (_packetLength <= 0) 
                    {
                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= _receivePackage.UnreadLength)
                {
                    byte[] _packetBytes = _receivePackage.ReadBytes(_packetLength);
                    _threadManager.ExecuteOnMainThread(() =>
                    {
                        using (BytePackage _packet = new BytePackage(_packetBytes)) {
                            int _packetId = _packet.ReadInt();
                            _serverRef.ActionsForPackage[_packetId](id, _packet);
                        }
                    });

                    _packetLength = 0;
                    if (_receivePackage.UnreadLength >= 4) {
                        _packetLength = _receivePackage.ReadInt();
                        if (_packetLength <= 0) { 
                            return true;
                        }
                    }
                }

                if (_packetLength <= 1) {
                    return true;
                }

                return false;
            }
        }
        private TCP _tcp;
    }
}
