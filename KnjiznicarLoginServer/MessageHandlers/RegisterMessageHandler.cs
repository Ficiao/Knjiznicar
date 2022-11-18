using KnjiznicarLoginServer.DB;
using KnjiznicarLoginServer.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class RegisterMessageHandler : BaseMessageHandler
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

            if(dbCredentials == null)
            {
                FirebaseDB.SendCredentialsToDB(playerCredentials);
                PlayerData playerData = new PlayerData()
                {
                    username = playerCredentials.username,
                    playerId = clientId,
                    level = 1,
                    items = new List<Item>(),
                    adventureLevel = 1,
                    pvpPoints = 0
                };
                FirebaseDB.SendDataToDB(playerData);
                Console.WriteLine($"Register successful for user {playerCredentials.username}.");
                LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage(true, false, new List<string> { "127.0.0.1" }, playerData);
                Server.Clients[clientId].Username = message.username;
                ServerSend.SendTCPData(clientId, loginSuccessful);
            }
            else
            {
                Console.WriteLine($"User {playerCredentials.username} already exists.");
                LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage(false, false);
                ServerSend.SendTCPData(clientId, loginSuccessful);
            }
        }
    }
}
