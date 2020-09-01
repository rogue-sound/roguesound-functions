using System;

namespace RogueSound.Common.Models
{
    public class RoomResponseModel
    {
        public Guid PublicId { get; set; }

        public string Creator { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Private { get; set; }

        public string Logo { get; set; }

        public string Cover { get; set; }
    }
}
