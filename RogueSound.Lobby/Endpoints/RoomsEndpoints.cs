using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Endpoints
{
    public class RoomsEndpoints
    {

        [FunctionName(nameof(GetRooms))]
        public async Task<IActionResult> GetRooms(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Rooms")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            return new OkResult();
        }

        [FunctionName(nameof(GetRoomDetails))]
        public async Task<IActionResult> GetRoomDetails(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Rooms/{roomId}")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            return new OkResult();
        }
    }
}
