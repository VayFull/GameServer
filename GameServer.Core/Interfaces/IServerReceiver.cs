using GameServer.Core.Models;

namespace GameServer.Core.Interfaces
{
    public interface IServerReceiver
    {
        public Server Server { get; set; }
        ReceivePacket ReceiveHelloPacket();
        ReceivePacket ReceivePositionPacket(string result);
        ReceivePacket ReceiveDisconnectPacket(string result);
    }
}