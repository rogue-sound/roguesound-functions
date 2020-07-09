using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSound.Functions
{
    public static class SongQueueExtensions
    {
        public static bool AnyUnplayed(this IEnumerable<SongQueueModel> songQueue, string songId)
        {
            return songQueue.Select(x => x.SongId == songId).Any();
        }

        public static IEnumerable<SongQueueModel> FindUnplayed(this IEnumerable<SongQueueModel> songQueue, string songId)
        {
            return songQueue.Where(x => x.SongId == songId && x.StartTime > DateTime.UtcNow).OrderBy(x => x.StartTime).ToList();
        }

        public static IEnumerable<SongQueueModel> RemoveUnplayed(this IEnumerable<SongQueueModel> songQueue, string songId)
        {
            var theOddOneOut = songQueue.Where(x => x.SongId == songId && x.StartTime > DateTime.UtcNow)
                .OrderBy(x => x.StartTime).FirstOrDefault();

            return songQueue.Except(new List<SongQueueModel>() { theOddOneOut })
                .FixQueueSongGapTimings(theOddOneOut);
        }

        public static IEnumerable<SongQueueModel> RemoveCurrent(this IEnumerable<SongQueueModel> songQueue)
        {
            var currentSong = songQueue.Where(x => DateTime.UtcNow > x.StartTime && DateTime.UtcNow < x.EndTime).FirstOrDefault();

           return songQueue.Except(new List<SongQueueModel>() { currentSong })
                .FixQueueSongGapTimings(currentSong);
        }

        public static IEnumerable<SongQueueModel> SkipCurrent(this IEnumerable<SongQueueModel> songQueue)
        {
            var currentSong = songQueue.Where(x => DateTime.UtcNow > x.StartTime && DateTime.UtcNow < x.EndTime).FirstOrDefault();

            return songQueue.FixQueueSongGapTimings(currentSong);
        }

        public static IEnumerable<SongQueueModel> FixQueueSongGapTimings(this IEnumerable<SongQueueModel> songQueue, SongQueueModel gappedSong)
        {
            var gapSpan = gappedSong.IsSongCurrent() ?
                TimeSpan.FromMilliseconds(gappedSong.Duration).Subtract(DateTime.UtcNow - gappedSong.StartTime) : TimeSpan.FromMilliseconds(gappedSong.Duration);

            songQueue.ToList()
                .ForEach(x =>
                {
                    x.EndTime = x.EndTime.Subtract(gapSpan);
                    x.StartTime = x.StartTime.Subtract(gapSpan);
                });

            return songQueue;
        }

        public static bool IsSongCurrent(this SongQueueModel song)
        {
            return DateTime.UtcNow > song.StartTime && DateTime.UtcNow < song.EndTime;
        }
    }
}
