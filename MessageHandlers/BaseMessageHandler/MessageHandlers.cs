using KnjiznicarDataModel.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KnjiznicarLoginServer.MessageHandlers
{ 
    public static class MessageHandlers
    {
        private static Dictionary<MessageType, BaseMessageHandler> _messageHandlers = new Dictionary<MessageType, BaseMessageHandler>()
        {
            { MessageType.Login, new LoginMessageHandler() },
            { MessageType.Register, new RegisterMessageHandler() },
            { MessageType.Logout, new LogoutMessageHandler() },
            { MessageType.PlayerConnected, new PlayerConnectedMessageHandler() },
            { MessageType.PlayerNameSelection, new PlayerNameSelectionHandler() },
            { MessageType.Error, new ErrorMessageHandler() },
            { MessageType.PlayerLoggedOut, new PlayerLoggedOutMessageHandler() },
        };

        public static void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            try
            {
                MessageType messageType = (MessageType)Int32.Parse(dataJsonObject["messageType"].ToString());
                _messageHandlers[messageType].HandleMessage(clientId, dataJsonObject, isServerMessage);
            }
            catch(Exception ex)
            {
                Console.Write($"Error processing message: {ex.Message}");
            }
        }
    }
}
