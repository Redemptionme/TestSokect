using AsyncServer;
using System;

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
