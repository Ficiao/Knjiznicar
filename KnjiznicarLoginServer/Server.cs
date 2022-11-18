using KnjiznicarDataModel.Message;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace KnjiznicarLoginServer
{
    class Server
    {
        public delegate void PacketHandler(int _fromClient, BaseMessage _packet);
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> Clients;

        private static TcpListener tcpListener;

        public static void Start(int maxPlayer, int port)
        {
            MaxPlayers = maxPlayer;
            Port = port;

            Console.WriteLine("Starting server...");

            InitializeServerData();
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

            Console.WriteLine($"Server started on {Port}.");
        }

        private static void TCPConnectCallBack(IAsyncResult ar)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(ar);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

            Console.WriteLine($"Incoming connection request from {client.Client.RemoteEndPoint}...");

            for(int i = 1; i <= MaxPlayers; i++)
            {
                if(Clients[i].Tcp.Socket == null)
                {
                    Clients[i].Tcp.Connect(client);
                    Console.WriteLine($"{client.Client.RemoteEndPoint} connected as id {i}.");
                    return;
                }
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server is full.");
        }

        private static void InitializeServerData()
        {
            Clients = new Dictionary<int, Client>();

            for (int i = 1; i <= MaxPlayers; i++) 
            {
                Clients.Add(i, new Client(i));
            }
        }
    }
}
