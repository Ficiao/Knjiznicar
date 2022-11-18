using KnjiznicarLoginServer.Enum;
using Newtonsoft.Json;

namespace KnjiznicarLoginServer.Message
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
