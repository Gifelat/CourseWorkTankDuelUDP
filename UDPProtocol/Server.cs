using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDPProtocol
{

    public class Server : ISocket
    {
        private static Server _instance;
        private IPAddress _localAddress = IPAddress.Parse("127.0.0.1");
        private int _port = GetAvailableUDPPort();
        private IPAddress _clientAddress;
        private int _clientPort;
        private Socket _sender;
        private Socket _receiver;
        private object _type = "Server";
        public object Type => _type;
        private bool _isReceive = false;
        public bool IsReceive => _isReceive;

        private bool _clientReady = false;
        public bool ClientReady => _clientReady;

        private string _tankInfo = "";
        public string TankInfo => _tankInfo;
        private string _bulletInfo = "";
        public string BulletInfo => _bulletInfo;


        /// <summary>
        /// Создание экземпляра класса сервера
        /// </summary>
        /// <returns>Экземпляр класса</returns>
        public static Server GetInstance()
        {
            if (_instance == null)
                _instance = new Server();
            return _instance;
        }

        public static Server GetInstance(string address, int port, bool freePort)
        {
            if (_instance == null)
                _instance = new Server(address, port, freePort);
            return _instance;
        }

        private Server()
        {
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Task.Run(ReceiveMessageAsync);
        }

        public Server(string address, int port, bool freePort)
        {
            _localAddress = IPAddress.Parse(address);
            _port = freePort ? GetAvailableUDPPort() : port;

            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Task.Run(ReceiveMessageAsync);
        }

        /// <summary>
        /// Метод для отправки сообщений клиенту
        /// </summary>
        /// <param name="message">Сообщение</param>
        async public void SendMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            byte[] data = Encoding.UTF8.GetBytes(message);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(data, 0, data.Length);
            args.RemoteEndPoint = new IPEndPoint(_clientAddress, _clientPort);

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
            _receiver.Bind(new IPEndPoint(_localAddress, _port));

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

                    if (message.Contains("ping"))
                    {
                        string[] split = message.Split(' ')[1].Split(':');
                        IPEndPoint senderEndpoint = (IPEndPoint)args.RemoteEndPoint;


                        if (split.Length > 1 && int.TryParse(split[1], out int port))
                        {
                            senderEndpoint.Port = port;
                            _clientAddress = senderEndpoint.Address;
                            _clientPort = port;

                            string response = $"done";
                            byte[] responseData = Encoding.UTF8.GetBytes(response);
                            SendMessageAsync(response);
                        }
                    }

                    else if (message.Contains("ClientReady"))
                    {
                        string[] split = message.Split(' ');
                        IPEndPoint senderEndpoint = (IPEndPoint)args.RemoteEndPoint;


                        if (split.Length > 1 && bool.TryParse(split[1], out bool value))
                        {
                            _clientReady = value;
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

        public static int GetAvailableUDPPort()
        {
            using (var udpClient = new UdpClient(0))
            {
                return ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
            }
        }

        public void ResetBulletInfo() => _bulletInfo = null;
        public void StopReceive() => _isReceive = false;
        public void StartReceive() => _isReceive = true;
        public void ClearInstance()
        {
            _receiver.Shutdown(SocketShutdown.Both);
            _receiver.Close();

            _isReceive = false;
            _clientReady = false;

            _instance = null;
        }

        public string GetInfo() => $"Type: {_type}; IP-address: {_localAddress}:{_port}";
        public string GetAddress() => $"{_localAddress}:{_port}";
    }
}
