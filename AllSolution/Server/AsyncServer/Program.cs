using System;

namespace AsyncServer
{
    class Program
    {
        static void Main(string[] args) {
            Console.WriteLine("AsyncTCPServer");
            AsyncTCPServer server = new AsyncTCPServer();
            server.Start();
            Console.ReadLine();
        }
    }
}
