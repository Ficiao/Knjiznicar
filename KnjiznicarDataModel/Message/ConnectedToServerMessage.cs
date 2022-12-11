using KnjiznicarDataModel.Enum;
using Newtonsoft.Json;

namespace KnjiznicarDataModel.Message
{
    class ConnectedToServerMessage : BaseMessage
    {
        [JsonProperty("welcomeMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string welcomeMessage;
        public ServerType serverType;

        public ConnectedToServerMessage() : base(MessageType.Connect)
        {
        }
    }
}
