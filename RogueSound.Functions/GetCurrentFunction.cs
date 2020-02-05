using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using System.Net.Http;
using Microsoft.Azure.Documents;
using System.Collections.Generic;

namespace RogueSound.Functions
{
    public static partial class RogueSoundFunctions
    {
        private static IConfigurationRoot config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

        private static DocumentClient client = GetCustomClient();
        private static DocumentClient GetCustomClient()
        {
            DocumentClient customClient = new DocumentClient(
                new Uri(config["CosmosEndpoint"]),
                config["CosmosKey"],
                new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp,
                    // Customize retry options for Throttled requests
                    RetryOptions = new RetryOptions()
                    {
                        MaxRetryAttemptsOnThrottledRequests = 10,
                        MaxRetryWaitTimeInSeconds = 30
                    }
                });

            return customClient;
        }

        [FunctionName("GetCurrent")]
        public static async Task<IActionResult> GetSong(
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
                .OrderBy( x => x.CreatedAt)
                .Take(1)
                .AsDocumentQuery();

            var sessionsReturned = new List<RoomSessionModel>();

            while (currentSessionQuery.HasMoreResults) sessionsReturned.AddRange(await currentSessionQuery.ExecuteNextAsync<RoomSessionModel>());

            var currentSession = sessionsReturned.FirstOrDefault();

            log.LogInformation($"Returned {sessionsReturned.Count} sessions");

            if (currentSession == null)
            {
                log.LogInformation($"Null current session, creating new");

                currentSession = new RoomSessionModel
                {
                    id = Guid.NewGuid().ToString(),
                    RoomId = 0,
                    SessionDate = todayDate,
                    Songs = Enumerable.Empty<SongQueueModel>()
                };

                await client.CreateDocumentAsync(queryUri, currentSession, partitionOptions);
            }

            log.LogInformation($"Retrieved {currentSession.Songs.Count()} songs");

            currentSession.Songs = currentSession.Songs.OrderByDescending(x => x.StartTime);

            return new OkObjectResult(currentSession.ToResponseModel());
        }

        
    }
}