using Cosmy;
using Microsoft.AspNetCore.Mvc;
using RogueSound.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Actions.Room
{
    public class DeleteRoomAction
    {
        private readonly ICosmyClient cosmyClient;

        public DeleteRoomAction(ICosmyClient cosmyClient)
        {
            this.cosmyClient = cosmyClient;
        }

        public async Task<IActionResult> ExecuteAsync(Guid roomId)
        {
            await this.cosmyClient.DeleteDocumentAsync(roomId, RoomConstants.Collection, RoomConstants.RoomPartitionKey);

            return new OkResult();
        }
    }
}
