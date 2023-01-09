using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class PlayerLoggedOutMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            if (isServerMessage == false) return;

            PlayerLoggedOutMessage message = JsonConvert.DeserializeObject<PlayerLoggedOutMessage>(dataJsonObject.ToString());
            if (Server.Clients.ContainsKey(message.Id))
            {
                Server.Clients[message.Id].ShouldKeep = false;
                Server.Clients[message.Id].Disconnect();
            }
        }
    }
}
