using System;

namespace AsyncClient
{
    class Program
    {
        static void Main(string[] args) {
            AsyncTCPClient client = new AsyncTCPClient();
            client.AsynConnect();
            Console.ReadLine();
        }
    }
}
