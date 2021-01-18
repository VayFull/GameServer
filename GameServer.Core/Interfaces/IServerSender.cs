using GameServer.Core.Models;

namespace GameServer.Core.Interfaces
{
    public interface IServerSender
    {
        public Server Server { get; set; }
        void SendClientId(SendPacket sendPacket);
        void SendAllClientsNewClient(SendPacket sendPacket);
        void SendAllClientsClientPosition(SendPacket sendPacket);
        void SendAllClientsClientDisconnect(SendPacket sendPacket);
    }
}