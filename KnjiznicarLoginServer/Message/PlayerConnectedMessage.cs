using KnjiznicarLoginServer.Enum;
using System.Net;

namespace KnjiznicarLoginServer.Message
{
    class PlayerConnectedMessage :BaseMessage
    {
        public PlayerData playerData;
        public string playerIp;

        public PlayerConnectedMessage() : base(MessageType.PlayerConnected)
        {

        }
    }
}
