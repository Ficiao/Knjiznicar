using KnjiznicarLoginServer.Enum;

namespace KnjiznicarLoginServer.Message
{
    class PlayerLoggedOutMessage : BaseMessage
    {
        public int id;
        public string ip;
        
        public PlayerLoggedOutMessage() : base(MessageType.PlayerLoggedOut)
        {

        }
    }
}
