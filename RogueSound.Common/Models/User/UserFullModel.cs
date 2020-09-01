using System.Collections.Generic;

namespace RogueSound.Common.Models
{
    public class UserFullModel
    {
        public string id { get; set; }

        public string UserIdentifier { get; set; }

        public string Name { get; set; }

        public string CustomAvatar { get; set; }

        public IEnumerable<string> Rooms { get; set; }
    }
}
