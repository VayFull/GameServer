using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace GameServer.Core.Models
{
    public class SendPacket
    {
        public int ClientId { get; set; }
        public IPEndPoint IpEndPoint { get; set; }
        public Dictionary<int, Client> Clients { get; set; }
        public byte[] Data { get; set; }
    }
}