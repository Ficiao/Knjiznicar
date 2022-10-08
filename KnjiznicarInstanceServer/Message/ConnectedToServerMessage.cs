using KnjiznicarInstanceServer.Enum;
using Newtonsoft.Json;

namespace KnjiznicarInstanceServer.Message
{
    class ConnectedToServerMessage : BaseMessage
    {
        [JsonProperty("welcomeMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string welcomeMessage;
        public int playerId;

        public ConnectedToServerMessage() : base(MessageType.Connect)
        {
            welcomeMessage = "Welcome to the server!";
        }
    }
}
