using RogueSound.Common.Models.Room;

namespace RogueSound.Common.Constants
{
    public static class RoomConstants
    {
        public const string Collection = "Rooms";
        public const string RoomPartitionKey = "RandomParty";

        public static RoomStyle[] RoomStyles = { new RoomStyle { Id = 1, Name = "RandomParty"}, new RoomStyle { Id = 2, Name = "Rock" } };
    }
}
