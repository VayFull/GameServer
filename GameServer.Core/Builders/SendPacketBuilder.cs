using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using GameServer.Core.Models;

namespace GameServer.Core.Builders
{
    public static class SendPacketBuilder
    {
        public static SendPacket HelloSendPacket(int clientId, IPEndPoint ipEndPoint, Dictionary<int, Client> clients)
        {
            var allClientsExceptCurrentClient = GetAllClientsExceptCurrentClient(clientId, clients);

            return new SendPacket
            {
                IpEndPoint = ipEndPoint,
                ClientId = clientId,
                Clients = allClientsExceptCurrentClient
            };
        }

        public static SendPacket AllClientsNewClientSendPacket(int clientId, Dictionary<int, Client> clients)
        {
            var allClientsExceptCurrentClient = GetAllClientsExceptCurrentClient(clientId, clients);

            return new SendPacket
            {
                ClientId = clientId,
                Clients = allClientsExceptCurrentClient
            };
        }

        public static SendPacket AllClientsClientPositionSendPacket(int clientId, byte[] data,
            Dictionary<int, Client> clients)
        {
            var allClientsExceptCurrentClient = GetAllClientsExceptCurrentClient(clientId, clients);

            return new SendPacket
            {
                ClientId = clientId,
                Data = data,
                Clients = allClientsExceptCurrentClient
            };
        }

        private static Dictionary<int, Client> GetAllClientsExceptCurrentClient(int clientId,
            Dictionary<int, Client> clients)
        {
            var allClientsExceptCurrentClient = clients
                .Where(x => x.Key != clientId)
                .ToDictionary(x => x.Key, x => x.Value);
            return allClientsExceptCurrentClient;
        }
    }
}