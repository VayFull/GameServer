using System.Numerics;
using GameServer.Core.Interfaces;
using GameServer.Core.Models;

namespace GameServer.Core.Handlers
{
    public class ServerReceiveHandler : IServerReceiver
    {
        public ServerReceiveHandler(Server server)
        {
            Server = server;
        }

        public Server Server { get; set; }
        public ReceivePacket ReceiveHelloPacket()
        {
            return null;
        }

        public ReceivePacket ReceivePositionPacket(string result)
        {
            var points = result
                .Split('?')[1]
                .Split(':');
            
            var position = new Vector3(float.Parse(points[0]), float.Parse(points[1]), float.Parse(points[2]));
            var clientId = int.Parse(result.Split('&')[0].Split('=')[1]);
            
            return new ReceivePacket
            {
                Position = position,
                ClientId = clientId
            };
        }

        public ReceivePacket ReceiveDisconnectPacket(string result)
        {
            var stringedId = result.Split(':')[1];
            return new ReceivePacket
            {
                ClientId = int.Parse(stringedId)
            };
        }
    }
}