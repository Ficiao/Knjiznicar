using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KnjiznicarDataModel.Enum;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class ErrorMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            ErrorMessage message = JsonConvert.DeserializeObject<ErrorMessage>(dataJsonObject.ToString());

            if (message.error == ErrorType.OverworldShutDown)
            {
                OverworldServer.Instance.Disconnect();
            }
        }
    }
}

