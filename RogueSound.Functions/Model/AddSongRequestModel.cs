using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Functions
{
    public class AddSongRequestModel
    {
        public string SongId { get; set; }

        public int Duration { get; set; }
        
        public string AlbumName { get; set; }
        
        public string AlbumImg { get; set; }
        
        public string Title { get; set; }
        
        public string Artist { get; set; }
        
        public string User { get; set; }
    }
}
