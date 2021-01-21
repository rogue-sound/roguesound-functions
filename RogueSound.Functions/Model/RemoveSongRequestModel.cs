using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Functions
{
    public class RemoveSongRequestModel
    {
        public string RoomStyle { get; set; }

        public string RoomId { get; set; }

        public string SongId { get; set; }
        
        public string User { get; set; }
    }
}
