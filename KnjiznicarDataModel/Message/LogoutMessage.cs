using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class LogoutMessage : BaseMessage
    {
        public bool responseNeeded;

        public LogoutMessage(bool responseNeeded) : base(MessageType.Logout)
        {
            this.responseNeeded = responseNeeded;
        }
    }
}
