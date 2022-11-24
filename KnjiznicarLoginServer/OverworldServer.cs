using System;
using System.Net;
using System.Net.Sockets;

namespace KnjiznicarLoginServer
{
    class OverworldServer : Singleton<OverworldServer>
    {
        private int _port;
        private static TcpListener _tcpListener;
        private ServerTCP _tcp;
        private int _dataBufferSize = 4096;
        private Action _callback;

        public ServerTCP TCP { get => _tcp; }

        public void Connect(int port, Action connectCallback)
        {
            _port = port;
            _callback = connectCallback;
            _tcpListener = new TcpListener(IPAddress.Loopback, port);
            _tcpListener.Start();
            Console.WriteLine("Connecting to overworld server...");
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);
            _tcp = new ServerTCP(_dataBufferSize, Reconnect);
        }

        private void TCPConnectCallBack(IAsyncResult ar)
        {
            TcpClient server = _tcpListener.EndAcceptTcpClient(ar);
            _tcp.Connect(server);
            _tcpListener.Stop();
            _callback();

            Console.WriteLine($"Connected to overworld server.");
        }

        private void Reconnect()
        {
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);
        }
    }
}
