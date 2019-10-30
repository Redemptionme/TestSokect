using common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsynServerNew
{
    class Program
    { 
        static void Main(string[] args) {
            AsynServerTcp server = new AsynServerTcp();
            server.start();
            Console.ReadLine(); 
        } 
    }
}
