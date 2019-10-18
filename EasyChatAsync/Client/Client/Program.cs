using AsyncClient;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            AsyncTCPClient client = new AsyncTCPClient();
            client.AsynConnect();

            while (true) ;
        }
    }
}
