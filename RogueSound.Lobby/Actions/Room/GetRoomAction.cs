using Cosmy;
using Microsoft.AspNetCore.Mvc;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using System;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Actions
{
    public class GetRoomAction
    {
        private readonly ICosmyClient cosmyClient;

        public GetRoomAction(ICosmyClient cosmyClient)
        {
            this.cosmyClient = cosmyClient;
        }

        public async Task<IActionResult> ExecuteAsync(string roomString)
        {
            if (string.IsNullOrEmpty(roomString)) return new BadRequestObjectResult("Null or empty room identifier received");

            var guidLenght = Guid.Empty.ToString().Length;

            var roomIdString = roomString.Substring(0, guidLenght);
            var styleString = roomString.Substring(guidLenght);

            var roomIdParseResult = Guid.TryParse(roomIdString, out var roomId);
            var styleParseResult = int.TryParse(styleString, out var style);

            if (!roomIdParseResult || !styleParseResult) return new BadRequestObjectResult("Invalid room identifier received");

            var doc = await this.cosmyClient.GetDocumentAsync<RoomModel>(roomId, RoomConstants.Collection, style);

            if (doc == null) return new NotFoundResult();

            var result = new RoomResponseModel
            {
                Id = doc.Id,
                Cover = doc.Cover,
                User = doc.User,
                Img = doc.Img,
                Description = doc.Description,
                Name = doc.Name
            };

            return new OkObjectResult(result);
        }
    }
}
