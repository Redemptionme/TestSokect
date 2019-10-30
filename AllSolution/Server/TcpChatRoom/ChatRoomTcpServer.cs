using common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChatRoomTcp
{
    public class ChatRoomTcpServer
    {
        static List<Client> clientList = new List<Client>();
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="message"></param>
        public static void BroadcastMessage(string message) {
            //存放没有连接的客户端
            var notConnectedList = new List<Client>();
            //遍历clientList列表，去发送消息
            foreach (var client in clientList) {
                //如果未断开连接，发送消息
                if (client.Connected)
                    client.SendMessage(message);
                else {
                    notConnectedList.Add(client);
                }
            }
            //移除没有连接的客户端
            foreach (var temp in notConnectedList) {
                clientList.Remove(temp);
            }
        }
        public void start() {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpServer.Bind(new IPEndPoint(IPAddress.Parse(Config.ip), Config.port));
            tcpServer.Listen(Config.listenNum);
            Console.WriteLine("服务器已开启");
            while (true) {
                Socket clientSocket = tcpServer.Accept();
                Console.WriteLine("一个用户已连接!");
                //把与每个客户端通信的逻辑(收发消息)放到client类里面进行处理
                Client client = new Client(clientSocket);
                //添加到集合中，可以得到服务器与那些客户端进行了连接
                clientList.Add(client);
            }
        }

    }
}
