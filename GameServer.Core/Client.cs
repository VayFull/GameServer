using System.Net;
using System.Numerics;

namespace GameServer.Core
{
    public class Client
    {
        public IPEndPoint IpEndpoint { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public Client(IPEndPoint ipEndpoint, Vector3 position, Vector3 rotation)
        {
            IpEndpoint = ipEndpoint;
            Position = position;
        }
    }
}