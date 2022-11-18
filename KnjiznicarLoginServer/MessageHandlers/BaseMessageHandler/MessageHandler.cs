using KnjiznicarLoginServer.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KnjiznicarLoginServer.MessageHandlers
{ 
    public static class MessageHandler
    {
        private static Dictionary<MessageType, BaseMessageHandler> _messageHandlers = new Dictionary<MessageType, BaseMessageHandler>()
        {
            { MessageType.Login, new LoginMessageHandler() },
            { MessageType.Register, new RegisterMessageHandler() },
            { MessageType.Logout, new LogoutMessageHandler() },
            { MessageType.PlayerConnected, new PlayerConnectedMessageHandler() },
        };

        public static void HandleMessage(int clientId, JObject dataJsonObject)
        {
            MessageType messageType = (MessageType)Int32.Parse(dataJsonObject["messageType"].ToString());
            _messageHandlers[messageType].HandleMessage(clientId, dataJsonObject);
        }
    }
}
