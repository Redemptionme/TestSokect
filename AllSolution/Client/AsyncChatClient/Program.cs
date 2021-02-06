using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace AsyncChatClient
{
    /// <summary>
    /// 聊天客户端
    /// </summary>
    class Program
    {
        /// <summary>
        /// 客户端Socket
        /// </summary>
        private static Socket client;
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
        /// 客户端名字
        /// </summary>
        private static string clientName;
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        private const string iPAddress = "127.0.0.1";
        /// <summary>
        /// 端口
        /// </summary>
        private const int port = 13333;

        static async Task Main(string[] args) {
            //创建客户端Socket
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //连接服务器
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(iPAddress), port);
            await client.ConnectAsync(iPEndPoint);
            Console.WriteLine("已连接到服务器:" + client.RemoteEndPoint);

            //接收服务器消息
            _ = ReceiveAsync();

            Console.WriteLine("输入你的名字:");
            clientName = Console.ReadLine();

            while (true) {
                //发送消息给服务器
                string msg = Console.ReadLine();
                sendBytes = Encoding.UTF8.GetBytes(clientName + ":" + msg);
                sendBuffer = new ArraySegment<byte>(sendBytes);
                await client.SendAsync(sendBuffer, SocketFlags.None);
            }
        }

        /// <summary>
        /// 接收服务器消息
        /// </summary>
        /// <returns></returns>
        static async Task ReceiveAsync() {
            try {
                receiveBuffer = new ArraySegment<byte>(receiveBytes);
                int countReceive = await client.ReceiveAsync(receiveBuffer, SocketFlags.None);
                string msg = Encoding.UTF8.GetString(receiveBuffer.Array, 0, countReceive);
                Console.WriteLine(DateTime.Now.ToString() + msg);
                _ = ReceiveAsync();
            }
            catch (Exception) {
                Console.WriteLine("服务器已关闭,请确认服务器开启后重启客户端...");
            }
        }
    }
}