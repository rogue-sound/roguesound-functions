using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSound.Functions
{
    public class SongQueueModel
    {
        public int RoomId { get; set; }

        public string SongId { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string AlbumName { get; set; }

        public string AlbumImg { get; set; }

        public double Duration { get; set; }

        public DateTime ResquestTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public static class SongQueueModelExtensions
    {
        public static IEnumerable<SongQueueResponseModel> ToResponseModel(this IEnumerable<SongQueueModel> songs)
        {
            var actualSongStart = songs.OrderByDescending(x => x.StartTime).FirstOrDefault(x => x.StartTime < DateTime.UtcNow).StartTime;

            return songs.Select(x => new SongQueueResponseModel
            {
                RoomId = x.RoomId,
                SongId = x.SongId,
                Title = x.Title,
                Artist = x.Artist,
                AlbumName = x.AlbumName,
                AlbumImg = x.AlbumImg,
                Duration = x.Duration,
                IsCurrent = (actualSongStart == x.StartTime) ? true : false,
                Position = (actualSongStart == x.StartTime) ? (DateTime.UtcNow - actualSongStart).TotalMilliseconds : 0,
                EndTime = x.EndTime,
                ResquestTime = x.ResquestTime,
                StartTime = x.StartTime
            });

        }
    }
}
