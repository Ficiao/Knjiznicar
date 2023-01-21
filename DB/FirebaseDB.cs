using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System;
using KnjiznicarDataModel;

namespace KnjiznicarLoginServer.DB
{
    public static class FirebaseDB
    {
        private static IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = Constants.DBAuthSecret,
            BasePath = Constants.DBBasePath,
        };

        private static IFirebaseClient dbClient = new FireSharp.FirebaseClient(fcon);

        public static void SendCredentialsToDB(PlayerCredentials playerCredentials)
        {
            try
            {
                SetResponse response = dbClient.Set("UserCredentials/" + playerCredentials.username, playerCredentials);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UpdateCredentialsOnDb(PlayerCredentials playerCredentials)
        {
            try
            {
                FirebaseResponse response = dbClient.Update("UserCredentials/" + playerCredentials.username, playerCredentials);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static PlayerCredentials GetCredentialsFromDB(string username)
        {
            try
            {
                FirebaseResponse response = dbClient.Get("UserCredentials/" + username);
                PlayerCredentials data = response.ResultAs<PlayerCredentials>();
                Console.WriteLine(response.StatusCode);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }        

        public static void DeleteData(PlayerCredentials playerData)
        {
            try
            {
                FirebaseResponse response = dbClient.Delete("UserData/" + playerData.username);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SendDataToDB(PlayerData playerData)
        {
            try
            {
                SetResponse response = dbClient.Set("UserData/" + playerData.PlayerName, playerData);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
