using AsyncServer;
using System;

// 目前此服务器无法接收多客户端
namespace Server
{
    class Program
    {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            AsyncTCPServer server = new AsyncTCPServer();
            server.Start();

            while (true) ;

        }
    }
}
