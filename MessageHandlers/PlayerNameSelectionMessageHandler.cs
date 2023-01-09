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
            bool playerNameNotValid = message.PlayerName.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.PlayerName.Length <= 0
                || message.PlayerName.Contains(username);
            PlayerCredentials playerCredentials = FirebaseDB.GetCredentialsFromDB(username);
            bool playerNameAlreadyExists = playerCredentials.playerName != null;

            if (clientNotLoggedIn || playerNameNotValid || playerNameAlreadyExists)
            {
                SendErrorMessage(clientId);
                return;
            }            

            playerCredentials.playerName = message.PlayerName;
            FirebaseDB.UpdateCredentialsOnDb(playerCredentials);
            PlayerData playerData = new PlayerData()
            {
                PlayerName = message.PlayerName,
                PlayerId = Guid.NewGuid().ToString(),
                Level = 1,
                Items = new List<Item>(),
                AdventureLevel = 0,
                PvpPoints = 0
            };
            playerData.Items.Add(new Item());
            FirebaseDB.SendDataToDB(playerData);

            PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
            {
                PlayerIp = Server.Clients[clientId].Tcp.Socket.Client.RemoteEndPoint.ToString().Split(':')[0],
                Username = playerCredentials.username,
                PlayerName = playerCredentials.playerName,
                ClientId = clientId
            };
            ServerSend.SendTCPDataToOverworldServer<PlayerConnectedMessage>(playerConnectedMessage);
        }

        private void SendErrorMessage(string clientId)
        {
            ErrorMessage errorMessage = new ErrorMessage()
            {
                Error = ErrorType.PlayerNameInvalid
            };
            ServerSend.SendTCPData(clientId, errorMessage);
        }
    }
}
