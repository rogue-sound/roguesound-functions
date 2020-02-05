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
using System.Collections.Generic;

namespace RogueSound.Functions
{
    public static partial class RogueSoundFunctions
    {
        [FunctionName("GetUpdates")]
        public static async Task<IActionResult> GetUpdatesFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var data = JsonConvert.DeserializeObject<RemoveSongRequestModel>(await req.ReadAsStringAsync());
            log.LogInformation($"User {data.User} dumbPolling call.");

            return new OkObjectResult(new { Lmao = "ayy" });
        }
    }
}
