using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;

class Server
{
    Socket _listenSocket;

    Dictionary<string, ClientProcess> _clients;

    public Server(int port) {
        _clients = new Dictionary<string, ClientProcess>();
        _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint lep = new IPEndPoint(IPAddress.Any, port);
        _listenSocket.Bind(lep);
    }

    public void Start() {
        _listenSocket.Listen(5);
        for (; ; )
        {
            Socket client = _listenSocket.Accept();
            ClientProcess cp = new ClientProcess(this, client);
            ThreadStart ts = new ThreadStart(cp.Process);
            Thread t = new Thread(ts);
            t.Start();
        }
    }

    public void AddClient(string email, ClientProcess c) {
        lock (_clients) {
            if (!_clients.ContainsKey(email))
                _clients.Add(email, c);
        }
    }

    public void RemoveClient(string email) {
        lock (_clients) {
            if (_clients.ContainsKey(email))
                _clients.Remove(email);
        }
    }

    public void SendTo(string from, string email, string msg) {
        ClientProcess cp;
        lock (_clients) {
            _clients.TryGetValue(email, out cp);
        }
        if (cp != null) {
            cp.Send(from, msg);
        }
    }
}

class ClientProcess
{
    Server _serv;
    Socket _s;

    TcpClient _tc;
    StreamReader _sr;
    StreamWriter _sw;

    string email;

    public ClientProcess(Server serv, Socket s) {
        _serv = serv;
        _s = s;
        _tc = new TcpClient();
        _tc.Client = _s;
        _sr = new StreamReader(_tc.GetStream());
        _sw = new StreamWriter(_tc.GetStream());
    }

    public void Process() {
        try {
            email = _sr.ReadLine();
            _serv.AddClient(email, this);

            for (; ; )
            {
                string l = _sr.ReadLine();
                int i = l.IndexOf(':');
                if (i == -1)
                    continue;

                string em = l.Substring(0, i);
                string msg = l.Substring(i + 1);
                _serv.SendTo(email, em, msg);
            }
        }
        catch {
            _serv.RemoveClient(email);
        }
    }

    public void Send(string email, string msg) {
        _sw.WriteLine("{0}:{1}", email, msg);
        _sw.Flush();
    }
}

class Program
{
    static void Main(string[] args) {
        Server s = new Server(12345);
        s.Start();
    }
}