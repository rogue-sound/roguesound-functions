using Cosmy;
using EzyPaging;
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

        public async Task<IActionResult> ExecuteAsync(PageModel pageModel)
        {
            var roomsQuery = this.cosmyClient.CreateDocumentQuery<RoomModel>(RoomConstants.Collection, RoomConstants.RoomPartitionKey).Where(x => x.Private == false).AddPaging(pageModel);
            var publicRooms = await roomsQuery.ExecuteQuery<RoomModel, RoomListResponseModel>();

            return new OkObjectResult(publicRooms);
        }
    }
}
