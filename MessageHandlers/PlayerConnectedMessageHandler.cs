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
            if (!Server.Clients[playerConnectedMessage.ClientId].Username.Equals(playerConnectedMessage.Username)) return;

            LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage()
            {
                LoginSuccessful = true,
                IsLogin = true,
                PlayerData = playerConnectedMessage.PlayerData,
                OverworldIp = Constants.overworldIp,
                OverworldPort = Constants.overworldPort,
                InstanceIp = Constants.instanceIp,
                InstancePort = Constants.instancePort
            };
            Console.WriteLine($"Login successful for user {playerConnectedMessage.PlayerData.PlayerName} as id {clientId}.");
            Server.Clients[playerConnectedMessage.ClientId].ShouldKeep = true;
            ServerSend.SendTCPData(playerConnectedMessage.ClientId, loginSuccessful);
        }
    }
}
