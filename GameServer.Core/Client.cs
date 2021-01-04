using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameServer.Core
{
    public class Client
    {
        private readonly UdpClient _udpClient;
        private IPEndPoint _serverEndpoint;
        public int? ClientId;

        public Client()
        {
            _udpClient = new UdpClient();
        }

        public IPEndPoint GetIpEndpoint() => (IPEndPoint) _udpClient.Client.LocalEndPoint;

        public void JoinServer(Server server)
        {
            _serverEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), server.Port);
            _udpClient.Connect(server.Hostname, server.Port);
            server.AddClient(this, ref ClientId);
            Console.WriteLine($"Client with Id {server.GetClientsCount()} successfully added to server!");

            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var receivedBytes = _udpClient.EndReceive(ar, ref _serverEndpoint);
            var result = Encoding.ASCII.GetString(receivedBytes);
            Console.WriteLine($"Received in client: {result}");
            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        public void SendBytesAsync(byte[] bytes) => _udpClient.BeginSend(bytes, bytes.Length, SendCallback, null);

        private void SendCallback(IAsyncResult ar)
        {
            _udpClient.EndSend(ar);
            Console.WriteLine("sent");
        }
    }
}