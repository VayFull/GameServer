using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameServer.Core
{
    public class Server
    {
        private readonly UdpClient _udpClient;
        public int Port { get; }
        public string Hostname { get; }
        private readonly ICollection<Client> _clients;

        public Server(string hostname, int port)
        {
            Port = port;
            Hostname = hostname;
            
            _udpClient = new UdpClient(port);
            _clients = new List<Client>();
            
            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        public void AddClient(Client client) => _clients.Add(client);

        private void ReceiveCallback(IAsyncResult ar)
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = _udpClient.EndReceive(ar, ref clientEndPoint);
            _udpClient.BeginReceive(ReceiveCallback, null);

            if (data.Length < 1)
                return;

            Console.WriteLine(Encoding.ASCII.GetString(data));
        }

        public void Broadcast(byte[] bytes)
        {
            foreach (var client in _clients)
            {
                var clientEndpoint = (IPEndPoint) client.GetUdpCLient().Client.LocalEndPoint;
                _udpClient.BeginSend(bytes, bytes.Length, clientEndpoint, SendCallback, null);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            _udpClient.EndSend(ar);
            Console.WriteLine("broadcast sent");
        }

        public int GetClientsCount() => _clients.Count;
    }
}