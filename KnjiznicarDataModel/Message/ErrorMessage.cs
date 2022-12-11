using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class ErrorMessage : BaseMessage
    {
        public ErrorType error;

        public ErrorMessage() : base(MessageType.Error)
        {
        }
    }
}
