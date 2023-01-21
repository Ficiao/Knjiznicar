using KnjiznicarDataModel.Message;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace KnjiznicarLoginServer
{
    class Server : Singleton<Server>
    {
        public delegate void PacketHandler(int _fromClient, BaseMessage _packet);
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<string, Client> Clients { get; private set; }

        private static TcpListener tcpListener;

        public static void Start(int maxPlayer, int port)
        {
            Clients = new Dictionary<string, Client>();
            MaxPlayers = maxPlayer;
            Port = port;

            Console.WriteLine("Starting server...");

            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

            Console.WriteLine($"Server started on {Port}.");
        }

        private static void TCPConnectCallBack(IAsyncResult ar)
        {
            try
            {
                TcpClient client = tcpListener.EndAcceptTcpClient(ar);
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

                Console.WriteLine($"Incoming connection request from {client.Client.RemoteEndPoint}...");

                if (Clients.Count < MaxPlayers)
                {
                    string id = Guid.NewGuid().ToString();
                    while (Clients.ContainsKey(id))
                    {
                        id = Guid.NewGuid().ToString();
                    }
                    Console.WriteLine($"{client.Client.RemoteEndPoint} connected as id {id}.");
                    Clients.Add(id, new Client(id));
                    Clients[id].Tcp.Connect(client);
                }
                else
                {
                    Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server is full.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error creating tcp connection {ex}");
            }
        }
    }
}
