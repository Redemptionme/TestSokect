using common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpChatRoom
{
    class ChatRoomTcpClient
    {
        Socket clientSocket;
        Thread t;
        readonly byte[] data = new byte[1024];
        //消息容器
        string message = "";

        public void start() {
            this.ConnectedToServer();
        }


        public void ConnectedToServer() {            
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //跟服务器端连接
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(Config.ip), Config.port));
            //创建一个新的线程用来接受消息
            t = new Thread(ReceiveMessage);
            t.Start();
        }
        /// <summary>
        /// 这个线程方法，用于循环接受消息
        /// </summary>
        void ReceiveMessage() {
            while (true) {
                if (clientSocket.Connected == false)
                    break;
                //获取得到的数据长度
                int lenght = clientSocket.Receive(data);
                //转化成字符串
                message = Encoding.UTF8.GetString(data, 0, lenght);
            }
        }
        void SendAMessage(string message) {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        void close() {
            //既不接受又不发送
            clientSocket.Shutdown(SocketShutdown.Both);
            //关闭连接
            clientSocket.Close();
        }


    }
}
