using EzyPaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RogueSound.Lobby.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Endpoints
{
    public class RoomsEndpoints
    {
        private readonly GetRoomsAction getRoomsAction;
        private readonly GetUserRoomsAction getUserRoomsAction;

        public RoomsEndpoints(GetRoomsAction getRoomsAction)
        {
            this.getRoomsAction = getRoomsAction;
        }

        [FunctionName(nameof(GetRooms))]
        public async Task<IActionResult> GetRooms(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Rooms")] HttpRequest req, ILogger log)
        {
            var paging = req.ExtractPaging();

            log.LogInformation("Serving request querying all public rooms");

            return await this.getRoomsAction.ExecuteAsync(paging);
        }

        [FunctionName(nameof(GetUserRooms))]
        public async Task<IActionResult> GetUserRooms(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Rooms/UserRooms")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Serving request for all user  rooms");

            return await this.getRoomsAction.ExecuteAsync();
        }

        [FunctionName(nameof(GetRoomDetails))]
        public async Task<IActionResult> GetRoomDetails(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Rooms/{roomId}")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            return new OkResult();
        }

        [FunctionName(nameof(AddRoom))]
        public async Task<IActionResult> AddRoom(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Rooms")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Serving request querying all rooms");

            return await this.getRoomsAction.ExecuteAsync();
        }
    }
}
