using System;

namespace TcpChatRoom
{
    class Program
    {
        static void Main(string[] args) {
            ChatRoomTcpClient client = new ChatRoomTcpClient();
            client.start();
            Console.ReadLine();
        }
    }
}
