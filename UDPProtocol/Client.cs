using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDPProtocol
{
    public class Client : ISocket
    {
        private static Client _instance;

        private IPAddress _localAddress = IPAddress.Parse("127.0.0.1");
        private int _port = GetAvailableUDPPort();
        public int Port => _port;
        private IPAddress _serverAddress;
        private int _serverPort;
        private Socket _sender;
        private Socket _receiver;
        private object _type = "Client";
        public object Type => _type;

        private bool _isReceive = false;
        public bool IsReceive => _isReceive;

        private bool _isConnection = false;
        public bool IsConnection => _isConnection;

        private bool _gameStart = false;
        public bool GameStart => _gameStart;

        private string _mapInfo = "";
        public string MapInfo => _mapInfo;

        private string _tankInfo = "";
        public string TankInfo => _tankInfo;

        private string _bulletInfo = "";
        public string BulletInfo => _bulletInfo;


        public static Client GetInstance()
        {
            if (_instance == null)
                _instance = new Client();

            return _instance;
        }

        public static Client GetInstance(string address, int port, bool freePort)
        {
            if (_instance == null)
                _instance = new Client(address, port, freePort);

            return _instance;
        }

        private Client()
        {
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver.Bind(new IPEndPoint(_localAddress, _port));

            Task.Run(ReceiveMessageAsync);
        }

        public Client(string address, int port, bool freePort)
        {
            _localAddress = IPAddress.Parse(address);
            _port = freePort ? GetAvailableUDPPort() : port;

            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver.Bind(new IPEndPoint(_localAddress, _port));

            Task.Run(ReceiveMessageAsync);
        }

        async public void SendMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            byte[] data = Encoding.UTF8.GetBytes(message);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(data, 0, data.Length);
            args.RemoteEndPoint = new IPEndPoint(_serverAddress, _serverPort);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            args.Completed += (s, e) =>
            {
                tcs.SetResult(true);
            };

            if (_sender.SendToAsync(args))
            {
                await tcs.Task;
            }
        }


        async public void ReceiveMessageAsync()
        {
            byte[] data = new byte[1024];
            _isReceive = true;

            while (_isReceive)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.SetBuffer(data, 0, data.Length);
                args.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                args.Completed += (s, e) =>
                {
                    var message = Encoding.UTF8.GetString(data, 0, args.BytesTransferred);
                    Console.WriteLine(message);

                    if (message.Equals("done"))
                    {
                        _isConnection = true;
                    }
                    else if (message.Equals("GameTrue"))
                    {
                        _gameStart = true;
                    }
                    else if (message.Contains("MapInfo"))
                    {
                        string[] split = message.Split(' ');
                        if (split.Length > 1)
                        {
                            _mapInfo = split[1];
                        }
                    }
                    else if (message.Contains("TankInfo"))
                    {
                        _tankInfo = message.Split(' ')[1];
                    }
                    else if (message.Contains("BulletInfo"))
                    {
                        string[] bulletInfo = message.Split(' ');
                        _bulletInfo = bulletInfo.Length > 1 ? bulletInfo[1] : "";
                    }

                    tcs.SetResult(true);
                };

                if (_receiver.ReceiveFromAsync(args))
                {
                    await tcs.Task;
                }
            }
        }

        public void Connection(string ip, int port, string message)
        {
            _isConnection = false;
            _serverAddress = IPAddress.Parse(ip);
            _serverPort = port;

            SendMessageAsync(message);
        }

        public static int GetAvailableUDPPort()
        {
            using (var udpClient = new UdpClient(0))
            {
                return ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
            }
        }

        public void ResetBulletInfo() => _bulletInfo = null;
        public void EndGame() => _gameStart = false;
        public void StopReceive() => _isReceive = false;
        public void StartReceive() => _isReceive = true;
        public void ClearInstance()
        {
            _receiver.Shutdown(SocketShutdown.Both);
            _receiver.Close();
            _isReceive = false;

            _isConnection = false;
            _gameStart = false;
            _mapInfo = "";

            _instance = null;
        }
        public string GetInfo() => $"Type: {_type}; IP-address: {_localAddress}:{_port}";
        public string GetAddress() => $"{_localAddress}:{_port}";
    }
}
