using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class DespawnPlayerMessage : BaseMessage
    {
        public string playerUsername;

        public DespawnPlayerMessage() : base(MessageType.DespawnPlayer)
        {
        }
    }
}
