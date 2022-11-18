using KnjiznicarLoginServer.Enum;
using KnjiznicarLoginServer.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;

namespace KnjiznicarLoginServer
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
                    case MessageType.Connect:
                        serializer.Serialize(writer, (ConnectedToServerMessage)message);
                        break;
                    case MessageType.LoginSuccessful:
                        serializer.Serialize(writer, (LoginSuccessfulMessage)message);
                        break;                
                    case MessageType.Logout:
                        serializer.Serialize(writer, (LogoutMessage)message);
                        break;
                    default:
                        break;
                }
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            Server.Clients[toClient].Tcp.SendData(sendData);
        }

        public static void SendTCPDataToOverworldServer(BaseMessage message)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                switch (message.messageType)
                {
                    case MessageType.Connect:
                        serializer.Serialize(writer, (ConnectedToServerMessage)message);
                        break;
                    case MessageType.LoginSuccessful:
                        serializer.Serialize(writer, (LoginSuccessfulMessage)message);
                        break;
                    case MessageType.Logout:
                        serializer.Serialize(writer, (LogoutMessage)message);
                        break;
                    case MessageType.PlayerConnected:
                        serializer.Serialize(writer, (PlayerConnectedMessage)message);
                        break;
                    case MessageType.PlayerLoggedOut:
                        serializer.Serialize(writer, (PlayerLoggedOutMessage)message);
                        break;
                    default:
                        Console.Write("Sending message of type without switch case.");
                        break;
                }
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            OverworldServer.Instance.TCP.SendData(sendData);
        }
    }
}
