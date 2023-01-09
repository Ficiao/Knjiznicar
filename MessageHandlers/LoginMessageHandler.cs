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
                username = message.Username,
                passwordHash = message.PasswordHash
            };

            if (message.Username.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.Username.Length <= 0
                || message.PasswordHash.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.PasswordHash.Length <= 0)
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    Error = ErrorType.PlayerCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }

            PlayerCredentials dbCredentials = FirebaseDB.GetCredentialsFromDB(playerCredentials.username);

            if (dbCredentials != null && dbCredentials.passwordHash == message.PasswordHash)
            {
                Client existingClient = Server.Clients.FirstOrDefault(c => c.Value.Username == message.Username).Value;
                if (existingClient != null)
                {
                    ServerSend.SendTCPDataToOverworldServer(new PlayerLoggedOutMessage()
                    {
                        Id = existingClient.Id,
                    });
                    existingClient.ShouldKeep = false;
                    existingClient.Disconnect();
                }

                if (dbCredentials.playerName != null)
                {
                    Server.Clients[clientId].Username = message.Username;
                    PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
                    {
                        PlayerIp = Server.Clients[clientId].Tcp.Socket.Client.RemoteEndPoint.ToString().Split(':')[0],
                        Username = playerCredentials.username,
                        PlayerName = dbCredentials.playerName,
                        ClientId = clientId,
                    };
                    ServerSend.SendTCPDataToOverworldServer<PlayerConnectedMessage>(playerConnectedMessage);
                }
                else
                {
                    Server.Clients[clientId].Username = message.Username;
                    LoginSuccessfulMessage loginMessage = new LoginSuccessfulMessage()
                    {
                        LoginSuccessful = true,
                        IsLogin = false,
                        Username = message.Username,
                    };
                    ServerSend.SendTCPData(clientId, loginMessage);
                }
            }
            else
            {
                Console.WriteLine($"User {playerCredentials.username} doesnt exist, or wrong credentials.");
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    Error = ErrorType.PlayerCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }
        }
    }
}
