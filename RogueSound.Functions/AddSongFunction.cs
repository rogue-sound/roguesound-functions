using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSound.Functions
{
    public partial class RogueSoundFunctions
    {

        [FunctionName("AddSong")]
        public static async Task<IActionResult> AddSong(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriger, adding new song");

            var songReq = JsonConvert.DeserializeObject<AddSongRequestModel>(await req.ReadAsStringAsync());

            if (string.IsNullOrEmpty(songReq.User)) return new BadRequestObjectResult("Request made from an unlogged client");

            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Sessions");
            var feedOptions = new FeedOptions { PartitionKey = new PartitionKey(songReq.RoomId) };

            var todayDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            var currentSessionQuery = client.CreateDocumentQuery<RoomSessionModel>(queryUri, feedOptions)
                .Where(x => x.SessionDate == todayDate)
                .OrderBy(x => x.CreatedAt)
                .Take(1)
                .AsDocumentQuery();

            var sessionsReturned = new List<RoomSessionModel>();

            while (currentSessionQuery.HasMoreResults) sessionsReturned.AddRange(await currentSessionQuery.ExecuteNextAsync<RoomSessionModel>());

            var currentSession = sessionsReturned.FirstOrDefault();

            log.LogInformation($"Returned {sessionsReturned.Count} sessions");

            if (currentSession == null) return new NotFoundResult();

            log.LogInformation($"FirstOrDefault session is {currentSession.id}");

            var songList = currentSession.Songs.OrderByDescending(x => x.StartTime).ToList();

            if (!songList.Any() || songList.FirstOrDefault().EndTime < DateTime.UtcNow)
            {
                log.LogInformation($"Queue already ended, StartTime set to now");

                songList.Insert(0, new SongQueueModel()
                {
                    PublicId = Guid.NewGuid(),
                    User = songReq.User ?? "anonymous",
                    SongId = songReq.SongId,
                    Artist = songReq.Artist,
                    AlbumName = songReq.AlbumName,
                    AlbumImg = songReq.AlbumImg,
                    RoomId = songReq.RoomId,
                    Title = songReq.Title,
                    RequestTime = DateTime.UtcNow,
                    Duration = songReq.Duration,
                    StartTime = DateTime.UtcNow.AddSeconds(1),
                    EndTime = DateTime.UtcNow.AddMilliseconds(songReq.Duration)
                });
            }
            else
            {
                songList.Insert(0, new SongQueueModel()
                {
                    PublicId = Guid.NewGuid(),
                    User = songReq.User ?? "anonymous",
                    SongId = songReq.SongId,
                    Artist = songReq.Artist,
                    AlbumName = songReq.AlbumName,
                    AlbumImg = songReq.AlbumImg,
                    RoomId = songReq.RoomId,
                    Title = songReq.Title,
                    RequestTime = DateTime.UtcNow,
                    Duration = songReq.Duration,
                    StartTime = songList.FirstOrDefault().EndTime.AddSeconds(1),
                    EndTime = songList.FirstOrDefault().EndTime.AddMilliseconds(songReq.Duration)
                });
            }

            currentSession.Songs = songList;

            var updateUri = UriFactory.CreateDocumentUri("RogueSound", "Sessions", currentSession.id);

            var partitionOptions = new RequestOptions { PartitionKey = new PartitionKey(songReq.RoomId) };

            await client.ReplaceDocumentAsync(updateUri, currentSession, partitionOptions);

            return new OkObjectResult(currentSession.ToResponseModel());
        }
    }
}
