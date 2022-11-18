using KnjiznicarLoginServer.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KnjiznicarLoginServer.Message
{
    class LoginSuccessfulMessage : BaseMessage
    {
        public bool loginSuccessful;
        public bool isLogin;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PlayerData playerData;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string overworldIp;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int overworldPort;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string instanceIp;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int instancePort;


        public LoginSuccessfulMessage(bool loginSuccessful, bool isLogin, List<string> ipList = null, PlayerData playerData = null) : base(MessageType.LoginSuccessful)
        {
            this.loginSuccessful = loginSuccessful;
            this.isLogin = isLogin;
            this.playerData = playerData;
            this.overworldIp = Constants.overworldIp;
            this.overworldPort = Constants.overworldPort;
            this.instanceIp = Constants.instanceIp;
            this.instancePort = Constants.instancePort;
        }
    }
}
