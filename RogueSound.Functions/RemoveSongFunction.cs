using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents;

namespace RogueSound.Functions
{
    public static partial class RogueSoundFunctions
    {
        [FunctionName("RemoveSong")]
        public static async Task<IActionResult> RemoveSong(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriger, removing song");

            var data = JsonConvert.DeserializeObject<RemoveSongRequestModel>(await req.ReadAsStringAsync());

            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Sessions");
            var feedOptions = new FeedOptions { PartitionKey = new PartitionKey(0) };

            var currentSessionQuery = client.CreateDocumentQuery<RoomSessionModel>(queryUri, feedOptions).AsDocumentQuery();
            var currentSession = (await currentSessionQuery.ExecuteNextAsync<RoomSessionModel>()).FirstOrDefault();

            var songList = currentSession.Songs.ToList();

            if (songList.AnyUnplayed(data.SongId))
            {
                var updatedSongList = songList.RemoveUnplayed(data.SongId);
            }

            return new OkResult();
        }
    }
}
