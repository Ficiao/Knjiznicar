using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class PlayerConnectedMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            if (isServerMessage == false) return;

            PlayerConnectedMessage playerConnectedMessage = JsonConvert.DeserializeObject<PlayerConnectedMessage>(dataJsonObject.ToString());
            if (!Server.Clients[playerConnectedMessage.clientId].Username.Equals(playerConnectedMessage.username)) return;

            LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage()
            {
                loginSuccessful = true,
                isLogin = true,
                playerData = playerConnectedMessage.playerData,
                overworldIp = Constants.overworldIp,
                overworldPort = Constants.overworldPort,
                instanceIp = Constants.instanceIp,
                instancePort = Constants.instancePort
            };
            Console.WriteLine($"Login successful for user {playerConnectedMessage.playerData.playerName} as id {clientId}.");
            ServerSend.SendTCPData(clientId, loginSuccessful);
        }
    }
}
