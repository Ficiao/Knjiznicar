using KnjiznicarInstanceServer.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;

namespace KnjiznicarInstanceServer
{
    class ServerSend
    {
        public static void SendTCPData(int toClient, BaseMessage message)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                switch (message.messageType)
                {
                    case Enum.MessageType.Connect:
                        serializer.Serialize(writer, (ConnectedToServerMessage)message);
                        break;
                    case Enum.MessageType.WordString:
                        serializer.Serialize(writer, (WordStringMessage)message);
                        break;
                    default:
                        break;
                }
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            Server.Clients[toClient].Tcp.SendData(sendData);
        }

        //public static void SendTCPDataToALl(BaseMessage message)
        //{
        //    for(int i = 1; i<= Server.MaxPlayers; i++)
        //    {
        //        Server.Clients[i].Tcp.SendData(message);
        //    }
        //}

        //public static void SendTCPDataToALl(int exceptClient, BaseMessage message)
        //{
        //    for (int i = 1; i <= Server.MaxPlayers; i++)
        //    {
        //        if (i == exceptClient) continue;
        //        Server.Clients[i].Tcp.SendData(message);
        //    }
        //}
    }
}
