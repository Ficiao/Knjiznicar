using KnjiznicarLoginServer.Enum;

namespace KnjiznicarLoginServer.Message
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
