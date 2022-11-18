using KnjiznicarLoginServer.DB;
using KnjiznicarLoginServer.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class LoginMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(int clientId, JObject dataJsonObject)
        {
            RegisterMessage message = JsonConvert.DeserializeObject<RegisterMessage>(dataJsonObject.ToString());
            PlayerCredentials playerCredentials = new PlayerCredentials()
            {
                username = message.username,
                passwordHash = message.passwordHash
            };

            PlayerCredentials dbCredentials = FirebaseDB.GetCredentialsFromDB(playerCredentials.username);

            if (dbCredentials != null ? dbCredentials.passwordHash == message.passwordHash : false)
            {
                Server.Clients[clientId].Username = message.username;
                PlayerData playerData = FirebaseDB.GetDataFromDB(playerCredentials.username);
                PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
                {
                    playerData = playerData,
                    playerIp = Server.Clients[clientId].Tcp.Socket.Client.RemoteEndPoint.ToString().Split(':')[0]
                };
                ServerSend.SendTCPDataToOverworldServer(playerConnectedMessage);
            }
            else
            {
                Console.WriteLine($"User {playerCredentials.username} doesnt exist, or wrong credentials.");
                LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage(false, true);
                ServerSend.SendTCPData(clientId, loginSuccessful);
            }
        }
    }
}
