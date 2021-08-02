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

        public ReceivePacket ReceivePositionRotationPacket(string result)
        {
            var positionPoints = result
                .Split('?')[1]
                .Split(':');
            
            var rotationPoints = result
                .Split('?')[2]
                .Split(':');
            
            var position = new Vector3(float.Parse(positionPoints[0]), float.Parse(positionPoints[1]), float.Parse(positionPoints[2]));
            var rotation = new Vector3(float.Parse(rotationPoints[0]), float.Parse(rotationPoints[1]), float.Parse(rotationPoints[2]));
            
            var clientId = int.Parse(result.Split('&')[0].Split('=')[1]);
            
            return new ReceivePacket
            {
                Position = position,
                ClientId = clientId,
                Rotation = rotation
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