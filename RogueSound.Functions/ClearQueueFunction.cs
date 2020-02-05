﻿using System;
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
using System.Collections.Generic;

namespace RogueSound.Functions
{
    public static partial class RogueSoundFunctions
    {
        [FunctionName("ClearQueue")]
        public static async Task<IActionResult> ClearQueueFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriger, clearing queue");

            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Sessions");
            var feedOptions = new FeedOptions { PartitionKey = new PartitionKey(0) };

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

            var songList = currentSession.Songs;

            if (songList.Any())
            {
                currentSession.Songs = Enumerable.Empty<SongQueueModel>();

                var uri = UriFactory.CreateDocumentUri("RogueSound", "Sessions", currentSession.id);

                var partitionOptions = new RequestOptions { PartitionKey = new PartitionKey(0) };

                await client.ReplaceDocumentAsync(uri, currentSession, partitionOptions);
            }

            return new OkResult();
        }
    }
}