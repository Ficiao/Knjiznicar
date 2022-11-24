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
                username = message.username,
                passwordHash = message.passwordHash
            };

            if(message.username.Count(c=> !char.IsLetterOrDigit(c)) > 0 || message.username.Length <= 0
                || message.passwordHash.Count(c => !char.IsLetterOrDigit(c)) > 0 || message.passwordHash.Length <= 0)
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    error = ErrorType.RegisterCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }

            PlayerCredentials dbCredentials = FirebaseDB.GetCredentialsFromDB(playerCredentials.username);

            if(dbCredentials == null)
            {
                FirebaseDB.SendCredentialsToDB(playerCredentials);                
                Console.WriteLine($"Register successful for user {playerCredentials.username}.");
                Server.Clients[clientId].Username = message.username;
                LoginSuccessfulMessage loginMessage = new LoginSuccessfulMessage()
                {
                    loginSuccessful = true,
                    isLogin = false,
                    username = message.username,
                };
                ServerSend.SendTCPData(clientId, loginMessage);
            }
            else
            {
                Console.WriteLine($"User {playerCredentials.username} already exists.");
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    error = ErrorType.RegisterCredentialsInvalid
                };
                ServerSend.SendTCPData(clientId, errorMessage);
            }
        }
    }
}
