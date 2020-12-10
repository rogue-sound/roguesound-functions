using System;

namespace RogueSound.Common.Models
{
    public class RoomModel
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public RoomUserModel User { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Private { get; set; }

        public string Password { get; set; }

        public string Img { get; set; }

        public string Cover { get; set; }

        public SessionConfiguration DefaultConfig { get; set; }

        public int Style { get; set; } = 1;
    }
}
