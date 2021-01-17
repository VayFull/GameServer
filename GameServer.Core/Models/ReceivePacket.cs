using System.Numerics;

namespace GameServer.Core.Models
{
    public class ReceivePacket
    {
        public int ClientId { get; set; }
        public Vector3 Position { get; set; }
    }
}