using KnjiznicarInstanceServer.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KnjiznicarInstanceServer.MessageHandlers
{ 
    class MessageHandler : Singleton<MessageHandler>
    {
        private Dictionary<MessageType, BaseMessageHandler> _messageHandlers = new Dictionary<MessageType, BaseMessageHandler>()
        {
            { MessageType.Login, new LoginMessageHandler() },
            { MessageType.Register, new RegisterMessageHandler() },
            { MessageType.WordString, null },
        };

        public void HandleMessage(JObject dataJsonObject)
        {
            _messageHandlers[(MessageType)Int32.Parse(dataJsonObject["messageType"].ToString())].HandleMessage(dataJsonObject);
        }
    }
}
