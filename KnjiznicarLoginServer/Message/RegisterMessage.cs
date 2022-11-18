using KnjiznicarLoginServer.Enum;
using Newtonsoft.Json;

namespace KnjiznicarLoginServer.Message
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
