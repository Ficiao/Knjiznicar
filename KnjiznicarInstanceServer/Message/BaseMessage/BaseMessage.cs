using KnjiznicarInstanceServer.Enum;

namespace KnjiznicarInstanceServer.Message
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
