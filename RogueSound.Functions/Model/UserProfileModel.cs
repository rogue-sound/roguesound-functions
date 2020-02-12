using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Functions.Model
{
    public class UserProfileModel
    {
        public string id { get; set; }

        public string UserIdentifier { get; set; }

        public string CustomName { get; set; }

        public string CustomPicture { get; set; }

        public int SongsPlayed { get; set; }
    }
}
