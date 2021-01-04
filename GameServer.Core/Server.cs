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
            _udpClient = new UdpClient(port);
            Port = port;
            Hostname = hostname;
            _clients = new List<Client>();

            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        public void AddClient(Client client) => _clients.Add(client);

        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = _udpClient.EndReceive(asyncResult, ref clientEndPoint);
            _udpClient.BeginReceive(ReceiveCallback, null);

            if (data.Length < 1)
                return;

            Console.WriteLine(Encoding.ASCII.GetString(data));
        }

        public int GetClientsCount() => _clients.Count;
    }
}