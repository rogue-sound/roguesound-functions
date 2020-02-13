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
    public static partial class RogueSoundFunctions
    {
        [FunctionName("GlobalPause")]
        public static async Task<IActionResult> GlobalPause(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Sessions");
            var feedOptions = new FeedOptions { PartitionKey = new PartitionKey(0) };
            var partitionOptions = new RequestOptions { PartitionKey = new PartitionKey(0) };

            var todayDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            log.LogInformation($"Querying session for {todayDate}");

            var currentSessionQuery = client.CreateDocumentQuery<RoomSessionModel>(queryUri, feedOptions)
                .Where(x => x.SessionDate == todayDate)
                .OrderBy(x => x.CreatedAt)
                .Take(1)
                .AsDocumentQuery();

            var sessionsReturned = new List<RoomSessionModel>();

            while (currentSessionQuery.HasMoreResults) sessionsReturned.AddRange(await currentSessionQuery.ExecuteNextAsync<RoomSessionModel>());

            var currentSession = sessionsReturned.FirstOrDefault();

            log.LogInformation($"Returned {sessionsReturned.Count} sessions");

            if (currentSession == null) return new BadRequestResult();

            currentSession.Songs = currentSession.Songs.OrderByDescending(x => x.StartTime);

            var currentSong = currentSession.Songs.FirstOrDefault(x => x.StartTime < DateTime.UtcNow);

            currentSession.IsPaused = true;
            currentSession.CurrentWhenPaused = currentSong.StartTime;

            currentSession.IsPaused = false;

            var updateUri = UriFactory.CreateDocumentUri("RogueSound", "Sessions", currentSession.id);

            await client.ReplaceDocumentAsync(updateUri, currentSession, partitionOptions);

            return new OkObjectResult(currentSession.ToResponseModel());

        }

        [FunctionName("GlobalResume")]
        public static async Task<IActionResult> GlobalResume(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
        {
            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Sessions");
            var feedOptions = new FeedOptions { PartitionKey = new PartitionKey(0) };
            var partitionOptions = new RequestOptions { PartitionKey = new PartitionKey(0) };

            var todayDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            log.LogInformation($"Querying session for {todayDate}");

            var currentSessionQuery = client.CreateDocumentQuery<RoomSessionModel>(queryUri, feedOptions)
                .Where(x => x.SessionDate == todayDate)
                .OrderBy(x => x.CreatedAt)
                .Take(1)
                .AsDocumentQuery();

            var sessionsReturned = new List<RoomSessionModel>();

            while (currentSessionQuery.HasMoreResults) sessionsReturned.AddRange(await currentSessionQuery.ExecuteNextAsync<RoomSessionModel>());

            var currentSession = sessionsReturned.FirstOrDefault();

            log.LogInformation($"Returned {sessionsReturned.Count} sessions");

            if (currentSession == null) return new BadRequestResult();

            currentSession.Songs = currentSession.Songs.OrderByDescending(x => x.StartTime).ToList();


            var gap = DateTime.UtcNow.Subtract(currentSession.PausedAt);

            foreach (var song in currentSession.Songs)
            {
                if (song.StartTime >= currentSession.CurrentWhenPaused)
                {
                    song.StartTime += gap;
                    song.EndTime += gap;
                }
            }


            var updateUri = UriFactory.CreateDocumentUri("RogueSound", "Sessions", currentSession.id);

            await client.ReplaceDocumentAsync(updateUri, currentSession, partitionOptions);

            return new OkObjectResult(currentSession.ToResponseModel());
        }
    }
}
