﻿using common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ThreadClient;

class Program
{  
    static void Main(string[] args) {

        ThreadSokectClient client = new ThreadSokectClient();
        client.start();
        Console.ReadLine();
    } 
}