using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class RegisterMessage : BaseMessage
    {
        public string username;
        public string passwordHash;

        public RegisterMessage() : base(MessageType.Register)
        {
        }
    }
}
