using Cosmy;
using EzyPaging;
using Microsoft.AspNetCore.Mvc;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using RogueSound.Common.Sorting;
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

        public async Task<IActionResult> ExecuteAsync(PageModel pageModel, SortModel sortModel)
        {
            var roomsQuery = this.cosmyClient.CreateDocumentQuery<RoomModel>(RoomConstants.Collection, RoomConstants.RoomPartitionKey)
                .AddSort(sortModel)
                .AddPaging(pageModel);

            var publicRooms = await roomsQuery.ExecuteQuery<RoomModel, RoomListResponseModel>();

            return new OkObjectResult(publicRooms);
        }
    }
}
