using System.Text;
using GameServer.Core;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server("localhost", 12000);
            
            var client = new Client();
            client.JoinServer(server);
            
            var client2 = new Client();
            client2.JoinServer(server);

            var sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
            var sendBytes2 = Encoding.ASCII.GetBytes("Is anybody therSWEGFSDDe?");
            
            client.SendBytesAsync(sendBytes);
            client2.SendBytesAsync(sendBytes2);
        }
    }
}