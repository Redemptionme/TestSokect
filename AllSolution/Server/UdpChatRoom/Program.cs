using System;

namespace UdpChatRoom
{
    class Program
    {
        static void Main(string[] args) {
            UdpChatRoomSever server = new UdpChatRoomSever();
            server.start();
            Console.ReadLine();
        }
    }
}
