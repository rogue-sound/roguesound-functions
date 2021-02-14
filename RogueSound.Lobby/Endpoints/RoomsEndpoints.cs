using EzyPaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using RogueSound.Common.Sorting;
using RogueSound.Lobby.Actions;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Endpoints
{
    public class RoomsEndpoints
    {
        private readonly GetRoomsAction getRoomsAction;
        private readonly AddRoomAction addRoomAction;
        private readonly GetRoomAction getRoomAction;
        private readonly DeleteRoomAction deleteRoomAction;

        public RoomsEndpoints(GetRoomsAction getRoomsAction, AddRoomAction addRoomAction,
            GetRoomAction getRoomAction, DeleteRoomAction deleteRoomAction)
        {
            this.getRoomsAction = getRoomsAction;
            this.addRoomAction = addRoomAction;
            this.getRoomAction = getRoomAction;
            this.deleteRoomAction = deleteRoomAction;
        }

        [FunctionName(nameof(GetRooms))]
        public async Task<IActionResult> GetRooms(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Rooms")] HttpRequest req, ILogger log)
        {
            var paging = req.ExtractPaging();
            var sorting = req.ExtractSorting();

            var styleValue = req.Query["style"];
            int.TryParse((string)styleValue, out int style);

            var searchName = (string)req.Query["name"];

            log.LogInformation("Serving request: GetRooms");

            return await this.getRoomsAction.ExecuteAsync(style, searchName ,paging, sorting);
        }

        [FunctionName(nameof(GetRoomDetails))]
        public async Task<IActionResult> GetRoomDetails(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Rooms/{roomId}")] HttpRequest req, string roomId, ILogger log)
        {
            log.LogInformation("Serving request: GetRoomDetails");

            return await this.getRoomAction.ExecuteAsync(roomId);
        }

        [FunctionName(nameof(GetStyles))]
        public IActionResult GetStyles(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Styles")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Serving request: GetStyles");

            return new OkObjectResult(RoomConstants.RoomStyles);
        }

        [FunctionName(nameof(AddRoom))]
        public async Task<IActionResult> AddRoom(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Rooms")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Serving request: add a new room");

            RoomCreateModel reqBody;
            try
            {
                reqBody = JsonConvert.DeserializeObject<RoomCreateModel>(await req.ReadAsStringAsync());
                if (reqBody == null) return new BadRequestObjectResult("Malformed request body");
            }
            catch
            {
                return new BadRequestObjectResult("Malformed request body");
            }

            return await this.addRoomAction.ExecuteAsync(reqBody);
        }

        [FunctionName(nameof(DeleteRoom))]
        public async Task<IActionResult> DeleteRoom(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Rooms/{roomId}")] HttpRequest req, string roomId, ILogger log)
        {
            log.LogInformation("Serving request: DeleteRoom");

            return await this.getRoomAction.ExecuteAsync(roomId);
        }
    }
}
