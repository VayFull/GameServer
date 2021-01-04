using System;
using System.Net.Sockets;

namespace GameServer.Core
{
    public class Client
    {
        private readonly UdpClient _udpClient;

        public Client()
        {
            _udpClient = new UdpClient();
        }

        public void JoinServer(Server server)
        {
            _udpClient.Connect(server.Hostname, server.Port);
            server.AddClient(this);
            Console.WriteLine($"Client with Id {server.GetClientsCount()} successfully added to server!");
        }

        public void SendBytesAsync(byte[] bytes) => _udpClient.BeginSend(bytes, bytes.Length, SendCallback, null);

        private void SendCallback(IAsyncResult ar)
        {
            _udpClient.EndSend(ar);
            Console.WriteLine("sent");
        }
    }
}