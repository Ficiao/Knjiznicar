using KnjiznicarLoginServer.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class PlayerConnectedMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(int clientId, JObject dataJsonObject)
        {
            PlayerConnectedMessage playerConnectedMessage = JsonConvert.DeserializeObject<PlayerConnectedMessage>(dataJsonObject.ToString());
            if (!Server.Clients[playerConnectedMessage.playerData.playerId].Username.Equals(playerConnectedMessage.playerData.username)) return;

            LoginSuccessfulMessage loginSuccessful = new LoginSuccessfulMessage(true, true, new List<string> { "127.0.0.1" }, playerConnectedMessage.playerData);
            Console.WriteLine($"Login successful for user {playerConnectedMessage.playerData.username} as id {playerConnectedMessage.playerData.playerId}.");
            ServerSend.SendTCPData(playerConnectedMessage.playerData.playerId, loginSuccessful);
        }
    }
}
