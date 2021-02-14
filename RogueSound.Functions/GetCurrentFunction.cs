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
using Newtonsoft.Json.Serialization;

namespace RogueSound.Functions
{
    public static partial class RogueSoundFunctions
    {
        private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();



        private static readonly DocumentClient Client = GetCustomClient();
        private static DocumentClient GetCustomClient()
        {
            DocumentClient customClient = new DocumentClient(
                new Uri(Config["CosmosEndpoint"]),
                Config["CosmosKey"],
                serializerSettings: new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                },
                new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetCurrent/{roomString}")] HttpRequest req, string roomString,
            ILogger log)
        {
            var guidLenght = Guid.Empty.ToString().Length;

            var roomIdString = roomString.Substring(0, guidLenght);
            var styleString = roomString.Substring(guidLenght);

            var roomIdParseResult = Guid.TryParse(roomIdString, out var roomId);
            var styleParseResult = int.TryParse(styleString, out var style);

            if (!roomIdParseResult || !styleParseResult) return new BadRequestObjectResult("Invalid room identifier received");

            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Sessions");
            var feedOptions = new FeedOptions { PartitionKey = new PartitionKey(roomIdString) };
            var partitionOptions = new RequestOptions { PartitionKey = new PartitionKey(roomIdString) };

            var todayDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            log.LogInformation($"Querying session for {todayDate}");

            var currentSessionQuery = Client.CreateDocumentQuery<RoomSessionModel>(queryUri, feedOptions)
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
                    RoomId = roomId.ToString(),
                    SessionDate = todayDate,
                    Songs = Enumerable.Empty<SongQueueModel>()
                };

                await Client.CreateDocumentAsync(queryUri, currentSession, partitionOptions);
            }

            var retardsUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Retards");
            var retardsQuery = Client.CreateDocumentQuery<RetardInSessionModel>(retardsUri, feedOptions)
                .Where(x => x.SessionId == currentSession.id)
                .OrderBy(x => x.DisplayName)
                .AsDocumentQuery();

            var retardsReturned = new List<RetardInSessionResponseModel>();
            while (retardsQuery.HasMoreResults) retardsReturned.AddRange(await retardsQuery.ExecuteNextAsync<RetardInSessionResponseModel>());

            var mockUser = "AvrilTheQueen";

            if (!retardsReturned.Any(x => x.DisplayName == mockUser))
            {
                var newRetard = new RetardInSessionModel(mockUser, null, currentSession.id, roomIdString);
                await Client.CreateDocumentAsync(retardsUri, newRetard, partitionOptions);

                retardsReturned.Add(new RetardInSessionResponseModel(mockUser, null));
            }


            log.LogInformation($"Retrieved {currentSession.Songs.Count()} songs");

            currentSession.Songs = currentSession.Songs.OrderByDescending(x => x.StartTime);

            return new OkObjectResult(currentSession.ToResponseModel());
        }
    }
}