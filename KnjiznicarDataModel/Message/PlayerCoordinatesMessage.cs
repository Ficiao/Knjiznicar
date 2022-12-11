using KnjiznicarDataModel.Enum;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KnjiznicarDataModel.Message
{
    class PlayerCoordinatesMessage : BaseMessage
    {
        public class PlayerCoordinates
        {
            [JsonProperty("b")]
            public string playerUsername;
            [JsonProperty("c")]
            public float[] position = new float[3];
            [JsonProperty("d")]
            public float[] rotation = new float[3];
            public int leftRightDirection;
            [JsonProperty("e")]
            public int forwardDirection;
            [JsonProperty("f")]
            public bool jump;
            [JsonProperty("g")]
            public bool grounded;
        }

        [JsonProperty("a")]
        public List<PlayerCoordinates> PlayerPositions;

        public PlayerCoordinatesMessage() : base(MessageType.PlayerCoordinates)
        {
            PlayerPositions = new List<PlayerCoordinates>();
        }
    }
}
