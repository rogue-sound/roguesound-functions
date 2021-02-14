using System;

namespace RogueSound.Functions
{
    class RetardInSessionModel
    {
        public RetardInSessionModel(string displayName, string avatar, string sessionId, string roomId)
        {
            Id = Guid.NewGuid().ToString();
            DisplayName = displayName;
            Avatar = avatar;
            SessionId = sessionId;
            RoomId = roomId;
        }

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public int Ttl { get; set; } = 10;
        public string SessionId { get; set; }
        public string RoomId { get; set; }
    }
}
