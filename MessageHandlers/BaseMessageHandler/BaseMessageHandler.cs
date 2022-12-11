using Newtonsoft.Json.Linq;

namespace KnjiznicarLoginServer.MessageHandlers
{
    public abstract class BaseMessageHandler
    {
        public abstract void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage);
    }
}
