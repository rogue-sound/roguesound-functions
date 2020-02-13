using Cosmy;
using Microsoft.AspNetCore.Mvc;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Actions
{
    public class GetRoomsAction
    {
        private readonly ICosmyClient cosmyClient;

        public GetRoomsAction(ICosmyClient cosmyClient)
        {
            this.cosmyClient = cosmyClient;
        }

        public async Task<IActionResult> ExecuteAsync()
        {
            var roomsQuery = this.cosmyClient.CreateDocumentQuery<RoomModel>(RoomConstants.Collection, RoomConstants.RoomPartitionKey).Where(x => x.Private == false);
            var publicRooms = await roomsQuery.ExecuteQuery<RoomModel, RoomResponseModel>();

            return new OkObjectResult(publicRooms);
        }
    }
}
