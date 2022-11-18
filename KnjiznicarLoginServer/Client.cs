using KnjiznicarDataModel.Message;
using System;

namespace KnjiznicarLoginServer
{
    public class Client
    {
        public static int DataBufferSize = 4096;
        public int Id;
        public string Username;
        public TCP Tcp;



        public Client(int id)
        {
            Id = id;
            Tcp = new TCP(id, DataBufferSize, this);
        }

        public void Disconnect()
        {
            if (Username == null) return;

            PlayerLoggedOutMessage playerLoggedOutMessage = new PlayerLoggedOutMessage()
            {
                id = Id,
                ip = Tcp.Socket.Client.RemoteEndPoint.ToString().Split(':')[0]
            };
            ServerSend.SendTCPDataToOverworldServer(playerLoggedOutMessage);

            Console.WriteLine($"{Username} from {Tcp.Socket.Client.RemoteEndPoint} with id {Id} has disconnected.");
            Username = null;

            Tcp.Disconnect();
        }
    }
}
