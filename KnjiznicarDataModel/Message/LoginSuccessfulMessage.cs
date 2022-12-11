using KnjiznicarDataModel.Enum;
using Newtonsoft.Json;

namespace KnjiznicarDataModel.Message
{
    class LoginSuccessfulMessage : BaseMessage
    {
        public bool loginSuccessful;
        public bool isLogin;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string username;
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


        public LoginSuccessfulMessage() : base(MessageType.LoginSuccessful)
        {
        }
    }
}
