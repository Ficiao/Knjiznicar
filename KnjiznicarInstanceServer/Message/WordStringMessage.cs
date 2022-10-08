using KnjiznicarInstanceServer.Enum;
using Newtonsoft.Json;

namespace KnjiznicarInstanceServer.Message
{
    class WordStringMessage : BaseMessage
    {
        [JsonProperty("wordString", NullValueHandling = NullValueHandling.Ignore)]
        public string wordString;

        public WordStringMessage() : base(MessageType.WordString)
        {
            wordString = null;
        }
    }
}
