using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Common.Models
{
    public class RoomListResponseModel
    {
        public string Id { get; set; }

        public RoomUserModel User { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public int Style { get; set; }
    }
}
