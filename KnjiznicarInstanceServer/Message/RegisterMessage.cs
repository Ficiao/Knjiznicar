using KnjiznicarInstanceServer.Enum;
using Newtonsoft.Json;

namespace KnjiznicarInstanceServer.Message
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
