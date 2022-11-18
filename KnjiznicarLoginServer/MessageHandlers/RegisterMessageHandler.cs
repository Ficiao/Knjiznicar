using KnjiznicarLoginServer.DB;
using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using KnjiznicarDataModel;

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
                LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage()
                {
                    loginSuccessful = true,
                    isLogin = false,
                    playerData = playerData,
                    overworldIp = Constants.overworldIp,
                    overworldPort = Constants.overworldPort,
                    instanceIp = Constants.instanceIp,
                    instancePort = Constants.instancePort
                };
                Server.Clients[clientId].Username = message.username;
                ServerSend.SendTCPData(clientId, loginSuccessful);
            }
            else
            {
                Console.WriteLine($"User {playerCredentials.username} already exists.");
                LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage()
                {
                    loginSuccessful = false,
                    isLogin = false
                };
                ServerSend.SendTCPData(clientId, loginSuccessful);
            }
        }
    }
}
