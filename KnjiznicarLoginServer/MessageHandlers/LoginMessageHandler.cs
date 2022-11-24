using KnjiznicarLoginServer.DB;
using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using KnjiznicarDataModel;
using System.Linq;
using KnjiznicarDataModel.Enum;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class LoginMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            LoginMessage message = JsonConvert.DeserializeObject<LoginMessage>(dataJsonObject.ToString());
            PlayerCredentials playerCredentials = new PlayerCredentials()
            {
                username = message.username,
                passwordHash = message.passwordHash
            };

            if (message.username.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.username.Length <= 0
                || message.passwordHash.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.passwordHash.Length <= 0)
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    error = ErrorType.PlayerCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }

            PlayerCredentials dbCredentials = FirebaseDB.GetCredentialsFromDB(playerCredentials.username);

            if (dbCredentials != null ? dbCredentials.passwordHash == message.passwordHash : false)
            {
                if (dbCredentials.playerName != null)
                {
                    Server.Clients[clientId].Username = message.username;
                    PlayerData playerData = FirebaseDB.GetDataFromDB(dbCredentials.playerName);
                    PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
                    {
                        playerData = playerData,
                        playerIp = Server.Clients[clientId].Tcp.Socket.Client.RemoteEndPoint.ToString().Split(':')[0],
                        username = playerCredentials.username
                    };
                    ServerSend.SendTCPDataToOverworldServer<PlayerConnectedMessage>(playerConnectedMessage);
                }
                else
                {
                    Server.Clients[clientId].Username = message.username;
                    LoginSuccessfulMessage loginMessage = new LoginSuccessfulMessage()
                    {
                        loginSuccessful = true,
                        isLogin = false,
                        username = message.username,
                    };
                    ServerSend.SendTCPData(clientId, loginMessage);
                }
            }
            else
            {
                Console.WriteLine($"User {playerCredentials.username} doesnt exist, or wrong credentials.");
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    error = ErrorType.PlayerCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }
        }
    }
}
