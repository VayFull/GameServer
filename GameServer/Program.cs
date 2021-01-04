using System;
using System.Text;
using System.Threading;
using GameServer.Core;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server("localhost", 12000);
            Console.WriteLine("Server successfully started...");
            Console.ReadKey();
        }
    }
}