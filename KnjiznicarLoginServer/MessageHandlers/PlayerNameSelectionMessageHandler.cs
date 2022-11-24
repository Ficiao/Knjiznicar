using KnjiznicarDataModel;
using KnjiznicarDataModel.Enum;
using KnjiznicarDataModel.Message;
using KnjiznicarLoginServer.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            if (username == null)
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    error = ErrorType.PlayerNameInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }

            if (message.playerName.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.playerName.Length <= 0
                || message.playerName.Contains(username))
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    error = ErrorType.PlayerNameInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }

            PlayerCredentials playerCredentials = FirebaseDB.GetCredentialsFromDB(username);
            if (playerCredentials.username != null)
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    error = ErrorType.PlayerNameInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }

            playerCredentials.playerName = message.playerName;
            FirebaseDB.UpdateCredentialsOnDb(playerCredentials);
            PlayerData playerData = new PlayerData()
            {
                playerName = message.playerName,
                playerId = clientId,
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
                username = playerCredentials.username
            };
            ServerSend.SendTCPDataToOverworldServer<PlayerConnectedMessage>(playerConnectedMessage);
        }
    }
}
