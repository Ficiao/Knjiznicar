using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class PlayerNameSelectionMessage : BaseMessage
    {
        public string playerName;

        public PlayerNameSelectionMessage() : base(MessageType.PlayerNameSelection)
        {
        }
    }
}
