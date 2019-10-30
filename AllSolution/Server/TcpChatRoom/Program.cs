using ChatRoomTcp;
using System;

namespace TcpChatRoom
{
    class Program
    {
        static void Main(string[] args) {
            ChatRoomTcpServer server = new ChatRoomTcpServer();
            server.start();
            Console.ReadLine();
        }
    }
}
