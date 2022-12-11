using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class UdpConnectMessage : BaseMessage
    {

        public UdpConnectMessage() : base(MessageType.UdpConnect)
        {
        }
    }
}
