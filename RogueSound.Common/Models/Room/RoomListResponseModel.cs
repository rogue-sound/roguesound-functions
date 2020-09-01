using System;

namespace RogueSound.Common.Models
{
    public class RoomListResponseModel
    {
        public Guid PublicId { get; set; }

        public string Creator { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Style { get; set; }

        public bool Private { get; set; }
    }
}
