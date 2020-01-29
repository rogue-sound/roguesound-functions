using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Functions
{
    public class SongQueueModel
    {
        public int RoomId { get; set; }

        public string SongId { get; set; }

        public string SongName { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Picture { get; set; }

        public double Duration { get; set; }

        public DateTime ResquestTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
