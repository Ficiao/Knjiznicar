using KnjiznicarDataModel.Enum;
using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;

namespace KnjiznicarLoginServer
{
    class ServerSend
    {
        public static void SendTCPData<T>(string toClient, T message) where T : BaseMessage
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, (T)message);
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            Server.Clients[toClient].Tcp.SendData(sendData);
        }

        public static void SendTCPDataToOverworldServer<T>(T message) where T : BaseMessage
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, (T)message);
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            OverworldServer.Instance.TCP.SendData(sendData);
        }
    }
}
