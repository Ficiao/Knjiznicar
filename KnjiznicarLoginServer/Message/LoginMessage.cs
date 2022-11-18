using KnjiznicarLoginServer.Enum;

namespace KnjiznicarLoginServer.Message
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
