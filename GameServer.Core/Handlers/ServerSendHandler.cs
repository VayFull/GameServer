using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using GameServer.Core.Interfaces;
using GameServer.Core.Models;

namespace GameServer.Core.Handlers
{
    public class ServerSendHandler : IServerSender
    {
        public ServerSendHandler(Server server)
        {
            Server = server;
        }

        public Server Server { get; set; }
        public void SendClientId(SendPacket sendPacket)
        {
            var result = $"id={sendPacket.ClientId}*";
            foreach (var packetClient in sendPacket.Clients)
            {
                var position = packetClient.Value.Position;
                result += $"{packetClient.Key},{position.X}:{position.Y}:{position.Z}";
            }

            var bytes = Encoding.ASCII.GetBytes(result);
            Server.Send(bytes, sendPacket.IpEndPoint);
        }

        public void SendAllClientsNewClient(SendPacket sendPacket)
        {
            var result = $"new:{sendPacket.ClientId}";
            var bytes = Encoding.ASCII.GetBytes(result);
            var group = GetGroupIpEndPoints(sendPacket);
            Server.GroupSend(bytes, group);
        }

        private static List<IPEndPoint> GetGroupIpEndPoints(SendPacket sendPacket)
        {
            var group = sendPacket.Clients.Values
                .Select(x => x.IpEndpoint)
                .ToList();
            return group;
        }

        public void SendAllClientsClientPosition(SendPacket sendPacket)
        {
            var group = GetGroupIpEndPoints(sendPacket);
            Server.GroupSend(sendPacket.Data, group);
        }
    }
}