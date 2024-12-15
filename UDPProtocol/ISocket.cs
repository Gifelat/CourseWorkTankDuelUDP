using System.Net.Sockets;
using System.Threading.Tasks;

namespace UDPProtocol
{
    public interface ISocket
    {
        string TankInfo { get; }
        string BulletInfo { get; }

        bool IsReceive { get; }
        object Type { get; }

        void ResetBulletInfo();
        void SendMessageAsync(string message);
        void ReceiveMessageAsync();

        void StopReceive();
        void StartReceive();

        void ClearInstance();

        string GetInfo();

        string GetAddress();
    }
}
