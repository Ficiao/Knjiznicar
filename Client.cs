using KnjiznicarDataModel.Message;
using System;

namespace KnjiznicarLoginServer
{
    public class Client
    {
        public static int DataBufferSize = 4096;
        public string Id;
        public string Username;
        public TCP Tcp;
        public bool ShouldKeep;

        public Client(string id)
        {
            Id = id;
            Tcp = new TCP(id, DataBufferSize, this);
        }

        public void Disconnect()
        {
            Tcp.Disconnect();
            if(ShouldKeep == false) Server.Clients.Remove(Id);
        }
    }
}
