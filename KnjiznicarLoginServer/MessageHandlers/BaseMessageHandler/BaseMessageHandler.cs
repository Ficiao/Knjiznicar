using Newtonsoft.Json.Linq;

namespace KnjiznicarLoginServer.MessageHandlers
{
    public abstract class BaseMessageHandler
    {
        public abstract void HandleMessage(int clientId, JObject dataJsonObject);
    }
}
