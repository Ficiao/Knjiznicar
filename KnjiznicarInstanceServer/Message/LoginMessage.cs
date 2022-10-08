using KnjiznicarInstanceServer.Enum;

namespace KnjiznicarInstanceServer.Message
{
    class LoginMessage : BaseMessage
    {
        public string username;
        public string passwordHash;

        public LoginMessage() : base(MessageType.Login)
        {
        }
    }
}
