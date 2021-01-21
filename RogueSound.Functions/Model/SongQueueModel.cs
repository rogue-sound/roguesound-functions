using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSound.Functions
{
    public class SongQueueModel
    {
        public Guid PublicId { get; set; }

        public string RoomId { get; set; }

        public string User { get; set; }

        public string SongId { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string AlbumName { get; set; }

        public string AlbumImg { get; set; }

        public double Duration { get; set; }

        public DateTime RequestTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public static class SongQueueModelExtensions
    {
        public static SongQueueResponseModel ToResponseModel(this SongQueueModel song)
        {
            if (song == null) return null;

            return new SongQueueResponseModel
            {
                PublicId = song.PublicId,
                User = song.User,
                RoomId = song.RoomId,
                SongId = song.SongId,
                Title = song.Title,
                Artist = song.Artist,
                AlbumName = song.AlbumName,
                AlbumImg = song.AlbumImg,
                Duration = song.Duration,
                Position = (DateTime.UtcNow - song.StartTime).TotalMilliseconds,
                EndTime = song.EndTime,
                ResquestTime = song.RequestTime,
                StartTime = song.StartTime
            };
        }
    }
}
