using Newtonsoft.Json.Linq;

namespace KnjiznicarInstanceServer.MessageHandlers
{
    public abstract class BaseMessageHandler
    {
        public abstract void HandleMessage(JObject dataJsonObject);
    }
}
