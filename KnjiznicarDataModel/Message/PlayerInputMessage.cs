using KnjiznicarDataModel.Enum;

namespace KnjiznicarDataModel.Message
{
    class PlayerInputMessage : BaseMessage
    {
        public int leftRightDirection;
        public int forwardDirection;
        public bool jump;
        public float[] rotation = new float[3];

        public PlayerInputMessage() : base(MessageType.PlayerInput)
        {
        }
    }
}
