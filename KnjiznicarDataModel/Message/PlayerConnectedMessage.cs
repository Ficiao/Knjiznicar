using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class PlayerConnectedMessage :BaseMessage
    {
        public PlayerData playerData;
        public string username;
        public string playerIp;
        public string clientId;

        public PlayerConnectedMessage() : base(MessageType.PlayerConnected)
        {

        }
    }
}
