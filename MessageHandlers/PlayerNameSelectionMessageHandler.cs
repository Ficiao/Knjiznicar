using KnjiznicarDataModel;
using KnjiznicarDataModel.Enum;
using KnjiznicarDataModel.Message;
using KnjiznicarLoginServer.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class PlayerNameSelectionHandler : BaseMessageHandler
    {
        public override void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            PlayerNameSelectionMessage message = JsonConvert.DeserializeObject<PlayerNameSelectionMessage>(dataJsonObject.ToString());
            string username = Server.Clients[clientId].Username;

            bool clientNotLoggedIn = username == null;
            bool playerNameNotValid = message.playerName.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.playerName.Length <= 0
                || message.playerName.Contains(username);
            PlayerCredentials playerCredentials = FirebaseDB.GetCredentialsFromDB(username);
            bool playerNameAlreadyExists = playerCredentials.playerName != null;

            if (clientNotLoggedIn || playerNameNotValid || playerNameAlreadyExists)
            {
                SendErrorMessage(clientId);
                return;
            }            

            playerCredentials.playerName = message.playerName;
            FirebaseDB.UpdateCredentialsOnDb(playerCredentials);
            PlayerData playerData = new PlayerData()
            {
                playerName = message.playerName,
                playerId = Guid.NewGuid().ToString(),
                level = 1,
                items = new List<Item>(),
                adventureLevel = 1,
                pvpPoints = 0
            };
            playerData.items.Add(new Item());
            FirebaseDB.SendDataToDB(playerData);

            PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
            {
                playerData = playerData,
                playerIp = Server.Clients[clientId].Tcp.Socket.Client.RemoteEndPoint.ToString().Split(':')[0],
                username = playerCredentials.username,
                clientId = clientId
            };
            ServerSend.SendTCPDataToOverworldServer<PlayerConnectedMessage>(playerConnectedMessage);
        }

        private void SendErrorMessage(string clientId)
        {
            ErrorMessage errorMessage = new ErrorMessage()
            {
                error = ErrorType.PlayerNameInvalid
            };
            ServerSend.SendTCPData(clientId, errorMessage);
        }
    }
}
