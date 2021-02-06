using System;

namespace ThreadChatRoom
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadChatRoomServer server = new ThreadChatRoomServer();
            server.start();
            Console.ReadLine();
        }
    }
}