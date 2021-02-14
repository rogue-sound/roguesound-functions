using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Common.Models
{
    public class RoomResponseModel
    {
        public string Id { get; set; }

        public RoomUserModel User { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public string Cover { get; set; }

        public int Style { get; set; }

    }
}
