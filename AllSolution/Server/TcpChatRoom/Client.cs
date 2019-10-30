using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatRoomTcp
{
    class Client
    {
        private Socket clientSocket;
        //创建一个线程
        private Thread t;
        //这个是一个数据容器
        private readonly byte[] data = new byte[1024];
        public Client(Socket s) {
            clientSocket = s;
            //启动一个线程 处理客户端的数据接收
            t = new Thread(ReceiveMessage);
            t.Start();
        }
        private void ReceiveMessage() {
            //一直接收客户端的数据，使用死循环
            while (true) {
                //在接收数据之前  判断一下socket连接是否断开(安全校验)
                //true 如果连接已关闭、 重置，或者终止，则返回
                if (clientSocket.Poll(10, SelectMode.SelectRead)) {
                    //关闭当前连接
                    clientSocket.Close();
                    break;//跳出循环 终止线程的执行
                }
                int length = clientSocket.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, length);
                //接收到数据的时候 要把这个数据 分发到客户端
                //广播这个消息
                ChatRoomTcpServer.BroadcastMessage(message);
                Console.WriteLine("收到了消息:" + message);
            }
        }
        //发送消息
        public void SendMessage(string message) {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }
        //判断是否连接
        public bool Connected {
            get { return clientSocket.Connected; }
        }
    }
}
