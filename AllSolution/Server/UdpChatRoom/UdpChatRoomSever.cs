using common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpChatRoom
{
    class UdpChatRoomSever
    {
        private static Socket udpServer;
        public void start() {
            //1、创建socket
            udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //2、绑定ip跟端口号
            udpServer.Bind(new IPEndPoint(IPAddress.Parse(Config.ip), Config.port));
            //3、接收数据
            new Thread(ReceiveMessage) { IsBackground = true }.Start();
            Console.ReadKey();
        }
        static void ReceiveMessage() {
            while (true) {
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = new byte[1024];
                //这个方法会把数据的来源(ip:port)放到第二个参数上
                int length = udpServer.ReceiveFrom(data, ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data, 0, length);
                Console.WriteLine("从IP：" + (remoteEndPoint as IPEndPoint).Address.ToString() + "：" + (remoteEndPoint as IPEndPoint).Port + "收到了数据：" + message);
            }
        }

    }
}
