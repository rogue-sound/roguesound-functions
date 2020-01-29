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
            return songQueue.Except(new List<SongQueueModel>(){theOddOneOut});
        }
    }
}
