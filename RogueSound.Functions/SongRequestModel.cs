using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Functions
{
    public class SongRequestModel
    {
        public int RoomId { get; set; }

        public string SongId { get; set; }

        public int Duration { get; set; }

        public DateTime ResquestTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
