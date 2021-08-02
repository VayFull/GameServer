using System;
using GameServer.Core;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("Введите hostname");
            var hostname = Console.ReadLine();
            Console.WriteLine("Введите порт");
            var port = int.Parse(Console.ReadLine());
            var server = new Server(hostname, port);*/
            var server = new Server("92.255.201.100", 25565);
            Console.WriteLine("Server successfully started...");
            Console.ReadKey();
        }
    }
}