using KnjiznicarLoginServer.DB;
using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using KnjiznicarDataModel;
using System.Linq;
using KnjiznicarDataModel.Enum;

namespace KnjiznicarLoginServer.MessageHandlers
{
    class RegisterMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(string clientId, JObject dataJsonObject, bool isServerMessage)
        {
            RegisterMessage message = JsonConvert.DeserializeObject<RegisterMessage>(dataJsonObject.ToString());
            PlayerCredentials playerCredentials = new PlayerCredentials()
            {
                username = message.Username,
                passwordHash = message.PasswordHash
            };

            if(message.Username.Count(c=> !char.IsLetterOrDigit(c)) > 0 || message.Username.Length <= 0
                || message.PasswordHash.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.PasswordHash.Length <= 0)
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    Error = ErrorType.RegisterCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }

            PlayerCredentials dbCredentials = FirebaseDB.GetCredentialsFromDB(playerCredentials.username);

            if(dbCredentials == null)
            {
                FirebaseDB.SendCredentialsToDB(playerCredentials);                
                Console.WriteLine($"Register successful for user {playerCredentials.username}.");
                Server.Clients[clientId].Username = message.Username;
                LoginSuccessfulMessage loginMessage = new LoginSuccessfulMessage()
                {
                    LoginSuccessful = true,
                    IsLogin = false,
                    Username = message.Username,
                };
                ServerSend.SendTCPData(clientId, loginMessage);
            }
            else
            {
                Console.WriteLine($"User {playerCredentials.username} already exists.");
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    Error = ErrorType.RegisterCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }
        }
    }
}
