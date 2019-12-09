using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncServerMesaage
{
    class Program
    {
        /// <summary>
        /// 实例化Message
        /// </summary>
        static Message rec_Message = new Message();
        static Socket serverSocket;
        static void Main(string[] args) {
            StartServer();
            //暂停
            Console.ReadKey();
        }
        /// <summary>
        /// 开启一个Socket
        /// </summary>
        static void StartServer() {
            //实例化一个Socket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //设置IP
            IPAddress ipAdress = IPAddress.Parse("127.0.0.1");

            //设置网络终结点
            IPEndPoint iPEndPoint = new IPEndPoint(ipAdress, 88);

            //绑定ip和端口号
            serverSocket.Bind(iPEndPoint);

            //等待队列(开始监听端口号)
            serverSocket.Listen(0);

            //异步接受客户端连接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }


        /// <summary>
        /// 开始发送数据到客户端
        /// </summary>
        /// <param name="toClientsocket">用以连接客户端的Socket</param>
        /// <param name="msg">要传递的数据</param>
        static void BeginSendMessagesToClient(Socket toClientsocket, string msg) {
            toClientsocket.Send(Message.GetBytes(msg));
        }

        /// <summary>
        /// 开始接收来自客户端的数据
        /// </summary>
        /// <param name="toClientsocket"></param>
        static void BeginReceiveMessages(Socket toClientsocket) {
            toClientsocket.BeginReceive(rec_Message.Data, rec_Message.StartIndex, rec_Message.RemindSize, SocketFlags.None, ReceiveCallBack, toClientsocket);
        }

        /// <summary>
        /// 当客户端连接到服务器时执行的回调函数
        /// </summary>
        /// <param name="ar"></param>
        static void AcceptCallBack(IAsyncResult ar) {
            //这里获取到的是向客户端收发消息的Socket
            Socket toClientsocket = serverSocket.EndAccept(ar);
            Console.WriteLine("客户端{0}连接进来了。", toClientsocket.RemoteEndPoint.ToString());
            //要向客户端发送的消息    
            string msg = "Hello client!你好。";
            //开始发送消息
            BeginSendMessagesToClient(toClientsocket, msg);
            //开始接收客户端传来的消息
            BeginReceiveMessages(toClientsocket);
            ////继续等待下一个客户端的链接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        /// <summary>
        /// 接收到来自客户端消息的回调函数
        /// </summary>
        /// <param name="ar"></param>
        static void ReceiveCallBack(IAsyncResult ar) {
            Socket toClientsocket = null;
            try {
                toClientsocket = ar.AsyncState as Socket;
                int count = toClientsocket.EndReceive(ar);
                if (count == 0) {
                    toClientsocket.Close();
                    return;
                }
                Console.WriteLine("从客户端：{0} 接收到数据,解析中。。。", toClientsocket.RemoteEndPoint);
                rec_Message.AddCount(count);
                //打印来自客户端的消息
                rec_Message.ReadMessage();
                BeginSendMessagesToClient(toClientsocket, "客户端" + toClientsocket.RemoteEndPoint + "我收到了你消息。");
                //继续监听来自客户端的消息
                toClientsocket.BeginReceive(rec_Message.Data, rec_Message.StartIndex, rec_Message.RemindSize, SocketFlags.None, ReceiveCallBack, toClientsocket);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                if (toClientsocket != null) {
                    toClientsocket.Close();
                }
            }
            finally {

            }

        }


    }
}
