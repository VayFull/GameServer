using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using GameServer.Core.Builders;
using GameServer.Core.Handlers;
using GameServer.Core.Models;

namespace GameServer.Core
{
    public class Server
    {
        private readonly UdpClient _udpClient;
        public int Port { get; }
        public string Hostname { get; }
        private readonly Dictionary<int, Client> _clients;
        private readonly ServerReceiveHandler _serverReceiveHandler;
        private readonly ServerSendHandler _serverSendHandler;

        public Server(string hostname, int port)
        {
            Port = port;
            Hostname = hostname;

            _serverReceiveHandler = new ServerReceiveHandler(this);
            _serverSendHandler = new ServerSendHandler(this);

            _udpClient = new UdpClient(port);
            _clients = new Dictionary<int, Client>();

            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        // ReSharper disable once RedundantAssignment
        public int AddAndGetClientId(Client client)
        {
            var newClientId = 0;
            if (_clients.Any())
                newClientId = _clients.Keys.Max() + 1;
            else
                newClientId = 1;

            _clients[newClientId] = client;
            Console.WriteLine($"Client with id {GetClientsCount()} successfully added");
            return newClientId;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = _udpClient.EndReceive(ar, ref clientEndPoint);

            if (data.Length < 1)
                return;

            var result = Encoding.ASCII.GetString(data);

            if (result.StartsWith("test:"))
            {
                var receivePacket = _serverReceiveHandler.ReceiveTestPacket(result);
                
                _serverSendHandler.SendTestPacket(
                    SendPacketBuilder.TestPacketSendPacket(clientEndPoint, receivePacket.ClientId));
            }

            if (result == "hello")
            {
                _serverReceiveHandler.ReceiveHelloPacket();

                var newClientId =
                    AddAndGetClientId(new Client(clientEndPoint, new Vector3(0, 2, 0), new Vector3(0, 2, 0)));
                _serverSendHandler.SendClientId(
                    SendPacketBuilder.HelloSendPacket(newClientId, clientEndPoint, _clients));

                _serverSendHandler.SendAllClientsNewClient(
                    SendPacketBuilder.AllClientsNewClientSendPacket(newClientId, _clients));
            }

            if (result.StartsWith("pos="))
            {
                var receivePacket = _serverReceiveHandler.ReceivePositionRotationPacket(result);
                _clients[receivePacket.ClientId].Position = receivePacket.Position;
                _clients[receivePacket.ClientId].Rotation = receivePacket.Rotation;

                _serverSendHandler.SendAllClientsClientPositionAndRotation(
                    SendPacketBuilder.AllClientsClientPositionSendPacket(receivePacket.ClientId, data, _clients));
            }

            if (result.StartsWith("disconnect:"))
            {
                var receivePacket = _serverReceiveHandler.ReceiveDisconnectPacket(result);
                _clients.Remove(receivePacket.ClientId);

                _serverSendHandler.SendAllClientsClientDisconnect(
                    SendPacketBuilder.AllClientsDisconnectClientSendPacket(receivePacket.ClientId, _clients));
            }

            Console.WriteLine($"server received: {Encoding.ASCII.GetString(data)}");
            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        public void Send(byte[] bytes, IPEndPoint clientEndPoints)
        {
            _udpClient.BeginSend(bytes, bytes.Length, clientEndPoints, SendCallback, null);
        }

        public void GroupSend(byte[] bytes, ICollection<IPEndPoint> clients)
        {
            foreach (var client in clients)
            {
                Send(bytes, client);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            _udpClient.EndSend(ar);
            Console.WriteLine("server sent callback");
        }

        public int GetClientsCount() => _clients.Count;
    }
}