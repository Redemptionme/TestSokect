using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using common;

namespace ThreadChatRoom
{
  public class ThreadChatRoomServer
  {
    private Socket SocketWatch = null;

    //定义一个集合，存储客户端信息
    static Dictionary<string, Socket> ClientConnectionItems = new Dictionary<string, Socket> { };

    public void start()
    {
      SocketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      SocketWatch.Bind(new IPEndPoint(IPAddress.Parse(Config.ip), Config.port));
      SocketWatch.Listen(Config.listenNum);
      Console.WriteLine("服务器已开启");
      
      //负责监听客户端的线程:创建一个监听线程 
      Thread threadwatch = new Thread(WatchConnecting);
      //将窗体线程设置为与后台同步，随着主线程结束而结束 
      threadwatch.IsBackground = true;
      //启动线程   
      threadwatch.Start();

      Console.WriteLine("开启监听......");
      Console.WriteLine("点击输入任意数据回车退出程序......");
      Console.ReadKey();

      SocketWatch.Close();
    }

    //监听客户端发来的请求 
    private void WatchConnecting()
    {
      Socket connection = null;

      //持续不断监听客户端发来的请求   
      while (true)
      {
        try
        {
          connection = SocketWatch.Accept();
        }
        catch (Exception ex)
        {
          //提示套接字监听异常   
          Console.WriteLine(ex.Message);
          break;
        }

        //客户端网络结点号 
        string remoteEndPoint = connection.RemoteEndPoint.ToString();
        //添加客户端信息 
        ClientConnectionItems.Add(remoteEndPoint, connection);
        //显示与客户端连接情况
        Console.WriteLine("\r\n[客户端\"" + remoteEndPoint + "\"建立连接成功！ 客户端数量：" + ClientConnectionItems.Count + "]");

        //获取客户端的IP和端口号 
        IPAddress clientIP = (connection.RemoteEndPoint as IPEndPoint).Address;
        int clientPort = (connection.RemoteEndPoint as IPEndPoint).Port;

        //让客户显示"连接成功的"的信息 
        string sendmsg = "[" + "本地IP：" + clientIP + " 本地端口：" + clientPort.ToString() + " 连接服务端成功！]";
        byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendmsg);
        connection.Send(arrSendMsg);

        //创建一个通信线程   
        Thread thread = new Thread(recv);
        //设置为后台线程，随着主线程退出而退出 
        thread.IsBackground = true;
        //启动线程   
        thread.Start(connection);
      }
    }

    /// <summary>
    /// 接收客户端发来的信息，客户端套接字对象
    /// </summary>
    /// <param name="socketclientpara"></param>  
    static void recv(object socketclientpara)
    {
      Socket socketServer = socketclientpara as Socket;

      while (true)
      {
        //创建一个内存缓冲区，其大小为1024*1024字节 即1M   
        byte[] arrServerRecMsg = new byte[1024 * 1024];
        //将接收到的信息存入到内存缓冲区，并返回其字节数组的长度  
        try
        {
          int length = socketServer.Receive(arrServerRecMsg);

          //将机器接受到的字节数组转换为人可以读懂的字符串   
          string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);

          //将发送的字符串信息附加到文本框txtMsg上   
          Console.WriteLine("\r\n[客户端：" + socketServer.RemoteEndPoint + " 时间：" +
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "]\r\n" + strSRecMsg);

          //Thread.Sleep(3000);
          //socketServer.Send(Encoding.UTF8.GetBytes("[" + socketServer.RemoteEndPoint + "]："+strSRecMsg));
          //发送客户端数据
          if (ClientConnectionItems.Count > 0)
          {
            foreach (var socketTemp in ClientConnectionItems)
            {
              socketTemp.Value.Send(Encoding.UTF8.GetBytes("[" + socketServer.RemoteEndPoint + "]：" + strSRecMsg));
            }
          }
        }
        catch (Exception)
        {
          ClientConnectionItems.Remove(socketServer.RemoteEndPoint.ToString());
          //提示套接字监听异常 
          Console.WriteLine("\r\n[客户端\"" + socketServer.RemoteEndPoint + "\"已经中断连接！ 客户端数量：" +
                            ClientConnectionItems.Count + "]");
          //关闭之前accept出来的和客户端进行通信的套接字 
          socketServer.Close();
          break;
        }
      }
    }
  }
}