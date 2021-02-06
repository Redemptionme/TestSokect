# TestSokect

# C# Socket编程 同步以及异步通信

[![复制代码](https://common.cnblogs.com/images/copycode.gif)](javascript:void(0);)

```
    套接字简介：套接字最早是Unix的，window是借鉴过来的。TCP/IP协议族提供三种套接字：流式、数据报式、原始套接字。其中原始套接字允许对底层协议直接访问，一般用于检验新协议或者新设备问题，很少使用。

套接字编程原理：延续文件作用思想，打开-读写-关闭的模式。

C/S编程模式如下：

Ø  服务器端：

打开通信通道，告诉本地机器，愿意在该通道上接受客户请求——监听，等待客户请求——接受请求，创建专用链接进行读写——处理完毕，关闭专用链接——关闭通信通道（当然其中监听到关闭专用链接可以重复循环）

Ø  客户端：打开通信通道，连接服务器——数据交互——关闭信道。

Socket通信方式：

Ø  同步：客户端在发送请求之后必须等到服务器回应之后才可以发送下一条请求。串行运行

Ø  异步：客户端请求之后，不必等到服务器回应之后就可以发送下一条请求。并行运行

套接字模式：

Ø  阻塞：执行此套接字调用时，所有调用函数只有在得到返回结果之后才会返回。在调用结果返回之前，当前进程会被挂起。即此套接字一直被阻塞在网络调用上。

Ø  非阻塞：执行此套接字调用时，调用函数即使得不到得到返回结果也会返回。

套接字工作步骤：

Ø  服务器监听：监听时服务器端套接字并不定位具体客户端套接字，而是处于等待链接的状态，实时监控网络状态

Ø  客户端链接：客户端发出链接请求，要连接的目标是服务器端的套接字。为此客户端套接字必须描述服务器端套接字的服务器地址与端口号。

Ø  链接确认：是指服务器端套接字监听到客户端套接字的链接请求时，它响应客户端链接请求，建立一个新的线程，把服务器端套接字的描述发送给客户端，一旦客户端确认此描述，则链接建立好。而服务器端的套接字继续处于监听状态，继续接受其他客户端套接字请求。

在TCP/IP网络中，IP网络交互分类两大类：面向连接的交互与面向无连接的交互。

 

 

 

Socket构造函数：public socket(AddressFamily 寻址类型, SocketType 套接字类型, ProtocolType 协议类型)。但需要注意的是套接字类型与协议类型并不是可以随便组合。

 

SocketType
 ProtocolType
 描述
 
Stream
 Tcp
 面向连接
 
Dgram
 Udp
 面向无连接
 
Raw
 Icmp
 网际消息控制
 
Raw
 Raw
 基础传输协议
 
Socket类的公共属性：
 
属性名
 描述
 
AddressFamily
 获取Socket的地址族
 
Available
 获取已经从网络接收且可供读取的数据量
 
Blocking
 获取或设置一个值，只是socket是否处于阻塞模式
 
Connected
 获取一个值，指示当前连接状态
 
Handle
 获取socket的操作系统句柄
 
LocalEndPoint
 获取本地终端EndPoint
 
RemoteEndPoint
 获取远程终端EndPoint
 
ProtocolType
 获取协议类型
 
SocketType
 获取SocketType类型
 
Socket常用方法：
 
Bind(EndPoint)
 服务器端套接字需要绑定到特定的终端，客户端也可以先绑定再请求连接
 
Listen(int)
 监听端口，其中parameters表示最大监听数
 
Accept()
 接受客户端链接，并返回一个新的链接，用于处理同客户端的通信问题
 
 
 
Send()
 发送数据
 
Send(byte[])
 简单发送数据
 
Send(byte[],SocketFlag)
 使用指定的SocketFlag发送数据
 
Send(byte[], int, SocketFlag)
 使用指定的SocketFlag发送指定长度数据
 
Send(byte[], int, int, SocketFlag)
 使用指定的SocketFlag，将指定字节数的数据发送到已连接的socket(从指定偏移量开始)
 
Receive()
 接受数据
 
Receive(byte[])
 简单接受数据
 
Receive (byte[],SocketFlag)
 使用指定的SocketFlag接受数据
 
Receive (byte[], int, SocketFlag)
 使用指定的SocketFlag接受指定长度数据
 
Receive (byte[], int, int, SocketFlag)
 使用指定的SocketFlag，从绑定的套接字接收指定字节数的数据，并存到指定偏移量位置的缓冲区
 
 
 
Connect(EndPoint)
 连接远程服务器
 
ShutDown(SocketShutDown)
 禁用套接字，其中SocketShutDown为枚举，Send禁止发送，Receive为禁止接受，Both为两者都禁止
 
Close()
 关闭套接字，释放资源
 
异步通信方法：
 
BeginAccept(AsynscCallBack,object)
 开始一个一步操作接受一个连接尝试。参数：一个委托。一个对象。对象包含此请求的状态信息。其中回调方法中必须使用EndAccept方法。应用程序调用BegineAccept方法后，系统会使用单独的线程执行指定的回调方法并在EndAccept上一直处于阻塞状态，直至监测到挂起的链接。EndAccept会返回新的socket对象。供你来同远程主机数据交互。不能使用返回的这个socket接受队列中的任何附加连接。调用BeginAccept当希望原始线程阻塞的时候，请调用WaitHandle.WaitOne方法。当需要原始线程继续执行时请在回调方法中使用ManualResetEvent的set方法
 
BeginConnect(EndPoint, AsyncCallBack, Object)
 回调方法中必须使用EndConnect()方法。Object中存储了连接的详细信息。
 
BeginSend(byte[], SocketFlag, AsyncCallBack, Object)
  
 
BegineReceive(byte[], SocketFlag, AsyncCallBack, Object)
  
 
BegineDisconnect(bool, AsyncCallBack, Object)
  
```