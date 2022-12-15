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
            if (Server.Clients.ContainsKey(message.id))
            {
                Server.Clients[message.id].ShouldKeep = false;
                Server.Clients[message.id].Disconnect();
            }
        }
    }
}
