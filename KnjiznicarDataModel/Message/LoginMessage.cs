using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
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
