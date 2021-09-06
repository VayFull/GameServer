using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GameServer.Const;
using NUnit.Framework;

namespace GameServer.Tests
{
    public class Tests
    {
        private UdpClient _udpClient;
        private IPEndPoint _serverEndpoint;
        
        private List<int> _results;

        private Dictionary<int, DateTime> _pingSends;
        private Dictionary<int, DateTime> _pingReceives;
        
        [SetUp]
        public void Setup()
        {
            _udpClient = new UdpClient();
            _results = new List<int>();
            _pingSends = new Dictionary<int, DateTime>();
            _pingReceives = new Dictionary<int, DateTime>();
            
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

            if (result.StartsWith("test"))
            {
                var pingNumber = int.Parse(result.Split(':')[1]);
                
                _results.Add(pingNumber);
                _pingReceives[pingNumber] = DateTime.Now;
            }

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
                var bytes = Encoding.ASCII.GetBytes($"test:{i}");
                _pingSends[i] = DateTime.Now;
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

        [Test]
        public void TestPing()
        {
            const int iterations = 100;
            
            var results = new List<int>();
            
            for (int i = 0; i < iterations; i++)
            {
                var bytes = Encoding.ASCII.GetBytes($"test:{i}");
                _pingSends[i] = DateTime.Now;
                _udpClient.BeginSend(bytes, bytes.Length, SendCallback, null);

                while (true)
                {
                    if (_pingReceives.ContainsKey(i))
                    {
                        break;
                    }
                }
                
                var difference = _pingReceives[i] - _pingSends[i];
                results.Add(difference.Milliseconds);
                TestContext.Out.WriteLine($"latency: {difference.Milliseconds} ms");
            }

            TestContext.Out.WriteLine($"average latency: {results.Average()} ms, max latency: {results.Max()}, min latency: {results.Min()}");
        }
    }
}