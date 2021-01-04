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
        private readonly ICollection<IPEndPoint> _clients;

        public Server(string hostname, int port)
        {
            Port = port;
            Hostname = hostname;
            
            _udpClient = new UdpClient(port);
            _clients = new List<IPEndPoint>();

            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        // ReSharper disable once RedundantAssignment
        public void AddClient(IPEndPoint client)
        {
            _clients.Add(client);
            Console.WriteLine($"Client with id {GetClientsCount()} successfully added");
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = _udpClient.EndReceive(ar, ref clientEndPoint);
            _udpClient.BeginReceive(ReceiveCallback, null);

            if (data.Length < 1)
                return;

            var result = Encoding.ASCII.GetString(data);

            if (result == "hello")
            {
                AddClient(clientEndPoint);
                SendClientId(GetClientsCount(), clientEndPoint);
            }

            Console.WriteLine(Encoding.ASCII.GetString(data));
        }

        private void SendClientId(int id, IPEndPoint client)
        {
            var bytes = Encoding.ASCII.GetBytes($"id:{id.ToString()}");
            Send(bytes, client);
        }

        public void Broadcast(byte[] bytes)
        {
            foreach (var client in _clients)
            {
                Send(bytes, client);
            }
        }

        private void Send(byte[] bytes, IPEndPoint clientEndPoints)
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
            Console.WriteLine("sent");
        }

        public int GetClientsCount() => _clients.Count;
    }
}