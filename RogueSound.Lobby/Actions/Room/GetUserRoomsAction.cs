using Cosmy;
using EzyPaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using RogueSound.Common.UserProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Actions
{
    public class GetUserRoomsAction
    {
        private readonly ICosmyClient cosmyClient;
        private readonly HttpContext httpContext;

        public GetUserRoomsAction(ICosmyClient cosmyClient, IHttpContextAccessor httpContextAccessor)
        {
            this.cosmyClient = cosmyClient;
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IActionResult> ExecuteAsync(PageModel pageModel)
        {
            var userId = this.httpContext.GetUserId();

            if (string.IsNullOrEmpty(userId)) return new BadRequestResult();

            var roomsQuery = this.cosmyClient.CreateDocumentQuery<UserFullModel>(UserConstants.Collection, UserConstants.RoomPartitionKey)
                .Where(x => x.UserIdentifier == userId)
                .SelectMany(x => x.Rooms);

            var userRooms = await roomsQuery.ExecuteQuery<string, string>();

            var roomList = new List<RoomListResponseModel>();

            foreach (var room in userRooms)
            {
                roomList.Add(await this.cosmyClient.GetDocumentAsync<RoomListResponseModel>(room, RoomConstants.Collection, RoomConstants.RoomPartitionKey));
            }

            return new OkObjectResult(roomList);
        }
    }
}
