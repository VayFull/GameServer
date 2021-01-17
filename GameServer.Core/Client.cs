using System.Net;
using System.Numerics;

namespace GameServer.Core
{
    public class Client
    {
        public IPEndPoint IpEndpoint { get; set; }
        public Vector3 Position { get; set; }

        public Client(IPEndPoint ipEndpoint, Vector3 position)
        {
            IpEndpoint = ipEndpoint;
            Position = position;
        }
    }
}