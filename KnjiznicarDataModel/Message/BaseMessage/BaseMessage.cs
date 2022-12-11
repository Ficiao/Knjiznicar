using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class BaseMessage
    {
        public MessageType messageType { get; }

        protected BaseMessage(MessageType type)
        {
            messageType = type;
        }
    }
}
