using Cosmy;
using Microsoft.AspNetCore.Mvc;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using System;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Actions.Room
{
    public class GetRoomAction
    {
        private readonly ICosmyClient cosmyClient;

        public GetRoomAction(ICosmyClient cosmyClient)
        {
            this.cosmyClient = cosmyClient;
        }

        public async Task<IActionResult> ExecuteAsync(Guid roomId, string style)
        {
            if (string.IsNullOrEmpty(style)) return new BadRequestObjectResult("Invalid style specified");
            
            try
            {
                var room = await this.cosmyClient.GetDocumentAsync<RoomModel>(roomId, RoomConstants.Collection, style);
                return new OkObjectResult(room);
            }
            catch
            {
                return new BadRequestObjectResult("Error retrieving room, check provied id");
            }
        }
    }
}
