using System;

namespace UdpChatRoom
{
    class Program
    {
        static void Main(string[] args) {
            UdpChatRoomClient client = new UdpChatRoomClient();
            client.start();
            Console.ReadLine();
        }
    }
}
