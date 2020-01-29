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

            var currentSessionQuery = client.CreateDocumentQuery<RoomSessionModel>(queryUri, feedOptions).AsDocumentQuery();

            var currentSession = (await currentSessionQuery.ExecuteNextAsync<RoomSessionModel>()).FirstOrDefault();

            if (!currentSession.Songs.Any()) return new NotFoundResult();

            var songList = currentSession.Songs;

            return new OkObjectResult(songList);
        }

        [FunctionName("AddSong")]
        public static async Task<IActionResult> AddSong(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriger, adding new song");

            var data = JsonConvert.DeserializeObject<AddSongRequestModel>(await req.ReadAsStringAsync());

            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", "Sessions");
            var feedOptions = new FeedOptions { PartitionKey = new PartitionKey(0) };

            var currentSessionQuery = client.CreateDocumentQuery<RoomSessionModel>(queryUri, feedOptions).AsDocumentQuery();

            var currentSession = (await currentSessionQuery.ExecuteNextAsync<RoomSessionModel>()).FirstOrDefault();

            var songList = currentSession.Songs;

            // Yay pole!
            if (!songList.Any())
            {
                var requestedSong = new SongQueueModel()
                {
                    SongId = data.SongId,
                    Artist = data.Artist,
                    AlbumName = data.AlbumName,
                    AlbumImg = data.AlbumImg,
                    RoomId = 0,
                    Title = data.Title,
                    ResquestTime = DateTime.UtcNow,
                    Duration = data.Duration,
                    StartTime = DateTime.UtcNow.AddSeconds(1),
                    EndTime = DateTime.UtcNow.AddMilliseconds(data.Duration)
                };

                var partitionOptions = new RequestOptions { PartitionKey = new PartitionKey(0) };
                await client.CreateDocumentAsync(queryUri, requestedSong, partitionOptions);

                return new OkObjectResult(new SongCurrentModel { SongId = data.SongId, TimerPosition = 0 });
            }
            else
            {
                var requestedSong = new SongQueueModel()
                {
                    SongId = data.SongId,
                    Artist = data.Artist,
                    AlbumName = data.AlbumName,
                    AlbumImg = data.AlbumImg,
                    RoomId = 0,
                    Title = data.Title,
                    ResquestTime = DateTime.UtcNow,
                    Duration = data.Duration,
                    StartTime = songList.FirstOrDefault().EndTime.AddSeconds(1),
                    EndTime = songList.FirstOrDefault().EndTime.AddMilliseconds(data.Duration)
                };

                var partitionOptions = new RequestOptions { PartitionKey = new PartitionKey(0) };
                await client.CreateDocumentAsync(queryUri, requestedSong, partitionOptions);
                
                var currentSong = songList.Where(x => x.StartTime <= DateTime.UtcNow).OrderByDescending(x => x.StartTime).FirstOrDefault();

                return new OkObjectResult(new SongCurrentModel { SongId = currentSong.SongId, TimerPosition = DateTime.UtcNow.Subtract(currentSong.StartTime).TotalMilliseconds });
            }
        }
    }
}