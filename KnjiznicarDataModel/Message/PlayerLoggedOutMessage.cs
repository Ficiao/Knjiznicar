using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class PlayerLoggedOutMessage : BaseMessage
    {
        public string id;
        
        public PlayerLoggedOutMessage() : base(MessageType.PlayerLoggedOut)
        {
        }
    }
}
