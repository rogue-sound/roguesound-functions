using System;

namespace RogueSound.Common.Models
{
    public class RoomCreateModel
    {
        public RoomUserModel User { get; set; }

        public string Name { get; set; }

        public int Style { get; set; }
    }

    public static class RoomCreateModelExtensions
    {
        public static RoomModel ToRoomModel(this RoomCreateModel room)
        {
            return new RoomModel
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                User = room.User,
                Name = room.Name,
                Style = room.Style
            };
        }
    }
}
