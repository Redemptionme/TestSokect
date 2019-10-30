using System;
using TCP_Client;

namespace AsynClientNew
{
    class Program
    {
        static void Main(string[] args) {
            for (int i = 0; i < 10; i++) {
                TCP_ClientAsync client = new TCP_ClientAsync();
                client.name = "端" + i;
                client.Start();
            }


            while (true) ;
        }
    }
}
