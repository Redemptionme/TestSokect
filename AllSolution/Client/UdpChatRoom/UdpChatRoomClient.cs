using common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpChatRoom
{
    class UdpChatRoomClient
    {
        public void start() {
            //创建socket
            Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            while (true) {
                //发送数据
                EndPoint serverPoint = new IPEndPoint(IPAddress.Parse(Config.ip), Config.port);
                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.SendTo(data, serverPoint);
            }
        }
    }
}
