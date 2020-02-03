using System;
using System.Collections.Generic;
using System.Linq;
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

    public class RoomSessionResponseModel
    {
        public int RoomId { get; set; } = 0;

        public DateTime SessionDate { get; set; }

        public SongQueueModel Current { get; set; }

        public IEnumerable<SongQueueModel> Songs { get; set; }
    }

    public static class RoomSessionModelExtensions
    {
        public static RoomSessionResponseModel ToResponseModel(this RoomSessionModel model)
        {
            var currentSong = model.Songs.FirstOrDefault(x => x.StartTime < DateTime.UtcNow);

            return new RoomSessionResponseModel
            {
                RoomId = model.RoomId,
                SessionDate = model.SessionDate,
                Current = (currentSong != null && currentSong.EndTime < DateTime.UtcNow)? null : currentSong,
                Songs = model.Songs
            };
        }
    }
}
