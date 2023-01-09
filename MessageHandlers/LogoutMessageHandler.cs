using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class LogoutMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            if (isServerMessage == false) return;

            LogoutMessage message = JsonConvert.DeserializeObject<LogoutMessage>(dataJsonObject.ToString());

            if (message.ResponseNeeded)
            {
                ServerSend.SendTCPData(clientId, new LogoutMessage(false));
            }

            Server.Clients[clientId].Disconnect();
        }
    }
}
