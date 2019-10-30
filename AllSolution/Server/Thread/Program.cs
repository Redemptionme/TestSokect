using common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ThreadServer;

class Program
{ 
    static void Main(string[] args) {
        ServerSokectThread server = new ServerSokectThread();
        server.start();
        Console.ReadLine();
    }
     
}