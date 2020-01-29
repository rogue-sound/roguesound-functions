using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Functions
{
    public class RoomSessionModel
    {
        public string id { get; set; }
            
        public int RoomId { get; set; } = 0;

        public DateTime SessionDate { get; set; }

        public IEnumerable<SongQueueModel> Songs { get; set; }
    }
}
