using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
namespace AsyncChatSever
{
    /// <summary>
    /// 聊天服务器
    /// </summary>
    class Program
    {
        /// <summary>
        /// 负责监听客户端连接的Socket
        /// </summary>
        private static Socket listener;
        /// <summary>
        /// 存储所有连接上的客户端列表
        /// </summary>
        private static List<Socket> clientList = new List<Socket>();
        /// <summary>
        /// 接收缓存
        /// </summary>
        private static ArraySegment<byte> receiveBuffer;
        private static byte[] receiveBytes = new byte[1024];
        /// <summary>
        /// 发送缓存
        /// </summary>
        private static ArraySegment<byte> sendBuffer;
        private static byte[] sendBytes;
        /// <summary>
        /// 端口
        /// </summary>
        private const int port = 13333;

        static async Task Main(string[] args)
        {
            //创建监听Socket
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //绑定IP、端口
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);
            listener.Bind(iPEndPoint);

            //开始监听
            listener.Listen(10);
            Console.WriteLine("等待客户端连接...");

            while (true)
            {
                //等待客户端连接
                Socket socket = await listener.AcceptAsync();
                Console.WriteLine(socket.RemoteEndPoint + "已连接...");

                //加入客户端列表
                clientList.Add(socket);
                //开始接收客户端消息
                _ = ReceiveAsync(socket);
            }
        }

        /// <summary>
        /// 接收客户端消息
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        static async Task ReceiveAsync(Socket socket)
        {
            try
            {
                //接收客户端消息
                receiveBuffer = new ArraySegment<byte>(receiveBytes);
                int countReceive = await socket.ReceiveAsync(receiveBuffer, SocketFlags.None);
                string msg = Encoding.UTF8.GetString(receiveBuffer.Array, 0, countReceive);
                Console.WriteLine(socket.RemoteEndPoint + ":" + msg);

                //将接收到的消息发送给其他客户端
                sendBytes = Encoding.UTF8.GetBytes(msg);
                sendBuffer = new ArraySegment<byte>(sendBytes);
                int countSend = await SendAllAsync(socket, sendBuffer);
                _ = ReceiveAsync(socket);
            }
            catch (Exception)
            {
                Console.WriteLine(socket.RemoteEndPoint + "已断开连接...");
                clientList.Remove(socket);
            }
        }

        /// <summary>
        /// 发送消息给其他客户端
        /// </summary>
        /// <param name="ignoreSocket"></param>
        /// <param name="sendBuffer"></param>
        /// <returns></returns>
        static async Task<int> SendAllAsync(Socket ignoreSocket, ArraySegment<byte> sendBuffer)
        {
            foreach (var client in clientList)
            {
                if (client != ignoreSocket)
                {
                    await client.SendAsync(sendBuffer, SocketFlags.None);
                }
            }
            return clientList.Count - 1;
        }
    }
}