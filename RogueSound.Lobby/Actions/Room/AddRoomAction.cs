using Cosmy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Actions
{
    public class AddRoomAction
    {
        private readonly ICosmyClient cosmyClient;

        public AddRoomAction(ICosmyClient cosmyClient)
        {
            this.cosmyClient = cosmyClient;
        }

        public async Task<IActionResult> ExecuteAsync(RoomCreateModel roomCreateModel)
        {
            var model = roomCreateModel.ToRoomModel();
            var result = await this.cosmyClient.CreateDocumentAsync(RoomConstants.Collection, model, model.Style);

            return (result != Guid.Empty)
                ? (IActionResult)new OkObjectResult(result)
                : (IActionResult)new BadRequestResult();
        }
    }
}
