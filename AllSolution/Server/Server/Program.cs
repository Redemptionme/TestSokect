using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program

    {

        static void Main()

        {

            try
            {

                int port = 2000;

                string host = "127.0.0.1";

                IPAddress ip = IPAddress.Parse(host);

                IPEndPoint ipe = new IPEndPoint(ip, port);

                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket类

                s.Bind(ipe);//绑定2000端口

                s.Listen(0);//开始监听

                Console.WriteLine("Wait for connect");

                Socket temp = s.Accept();//为新建连接创建新的Socket。

                Console.WriteLine("Get a connect");

                string recvStr = "";

                byte[] recvBytes = new byte[1024];

                int bytes;

                bytes = temp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息

                recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);

                Console.WriteLine("Server Get Message:{0}", recvStr);//把客户端传来的信息显示出来

                string sendStr = "Ok!Client Send Message Sucessful!";

                byte[] bs = Encoding.ASCII.GetBytes(sendStr);

                temp.Send(bs, bs.Length, 0);//返回客户端成功信息

                temp.Close();

                s.Close();

            }

            catch (ArgumentNullException e)
            {

                Console.WriteLine("ArgumentNullException: {0}", e);
            }

            catch (SocketException e)
            {

                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("Press Enter to Exit");

            Console.ReadLine();

        }

    }
}
