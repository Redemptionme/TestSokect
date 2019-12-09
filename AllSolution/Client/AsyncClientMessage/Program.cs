using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace AsyncClientMessage
{
    class Program
    {
        /// <summary>
        /// 实例化Message
        /// </summary>
        static Message rec_Message = new Message();
        //声明客户端
        static Socket clientSocket;
        static void Main(string[] args) {
            StartClient();
            while (true) {
                Console.WriteLine("请输入想要向服务器发送的字符串：");
                string data = Console.ReadLine();
                BeginSendMessagesToServer(data);
            }
        }
        /// <summary>
        /// 开启客户端并连接到服务器端
        /// </summary>
        static void StartClient() {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 88));
                Console.WriteLine("连接服务器成功");
            }
            catch (Exception e) {
                Console.WriteLine(e);
                Console.WriteLine("连接服务器失败");
            }
            BeginSendMessagesToServer("Hello,服务器端。");
            BeginReceiveMessages();
        }

        /// <summary>
        /// 开始发送数据到客户端
        /// </summary>
        /// <param name="msg">要传递的数据</param>
        static void BeginSendMessagesToServer(string msg) {
            try {
                clientSocket.Send(Message.GetBytes(msg));
                Console.WriteLine("{0} 发送成功!", msg);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 开始接收来自客户端的数据
        /// </summary>
        /// <param name="toClientsocket"></param>
        static void BeginReceiveMessages() {
            clientSocket.BeginReceive(rec_Message.Data, rec_Message.StartIndex, rec_Message.RemindSize, SocketFlags.None, ReceiveCallBack, null);
        }

        /// <summary>
        /// 接收到来自服务端消息的回调函数
        /// </summary>
        /// <param name="ar"></param>
        static void ReceiveCallBack(IAsyncResult ar) {
            try {
                int count = clientSocket.EndReceive(ar);
                Console.WriteLine("从客户端接收到数据,解析中。。。");
                rec_Message.AddCount(count);
                //打印来自客户端的消息
                rec_Message.ReadMessage();

                //继续监听来自服务端的消息
                clientSocket.BeginReceive(rec_Message.Data, rec_Message.StartIndex, rec_Message.RemindSize, SocketFlags.None, ReceiveCallBack, null);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }

        }
    }
}
