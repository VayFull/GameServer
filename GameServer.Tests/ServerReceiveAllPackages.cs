using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GameServer.Const;
using NUnit.Framework;

namespace GameServer.Tests
{
    public class Tests
    {
        private UdpClient _udpClient;
        private IPEndPoint _serverEndpoint;
        private List<int> _results;
        
        [SetUp]
        public void Setup()
        {
            _udpClient = new UdpClient();
            _results = new List<int>();
            
            var hostname = ServerConstants.Hostname;
            var port = ServerConstants.Port;
            _serverEndpoint = new IPEndPoint(IPAddress.Parse(hostname), port);
            
            _udpClient.Connect(hostname, port);
            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var receivedBytes = _udpClient.EndReceive(ar, ref _serverEndpoint);
            var result = Encoding.ASCII.GetString(receivedBytes);
            TestContext.Out.WriteLine($"client received: {result}");
            
            _results.Add(int.Parse(result.Split(':')[1]));
            
            _udpClient.BeginReceive(ReceiveCallback, null);
        }
        
        private void SendCallback(IAsyncResult ar)
        {
            _udpClient.EndSend(ar);
        }

        [Test]
        public void AllPackageReceived()
        {
            const int iterations = 1000;
            
            for (int i = 0; i < iterations; i++)
            {
                TestContext.Out.WriteLine($"test:{i}");
                var bytes = Encoding.ASCII.GetBytes($"test:{i}");
                _udpClient.BeginSend(bytes, bytes.Length, SendCallback, null);
            }

            while (_results.Count != iterations)
            {
                
            }

            for (int i = 0; i < iterations - 1; i++)
            {
                Assert.True(_results[i + 1] - _results[i] == 1);
            }
            
            Assert.True(_results.Count == iterations);
        }
    }
}