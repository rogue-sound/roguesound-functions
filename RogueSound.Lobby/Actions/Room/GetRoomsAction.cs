using Cosmy;
using EzyPaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using RogueSound.Common.Constants;
using RogueSound.Common.Models;
using RogueSound.Common.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RogueSound.Lobby.Actions
{
    public class GetRoomsAction
    {
        private readonly DocumentClient documentClient;

        public GetRoomsAction(DocumentClient documentClient)
        {
            this.documentClient = documentClient;
        }

        public async Task<IActionResult> ExecuteAsync(int style, string searchName, PageModel pageModel, SortModel sortModel)
        {
            if (pageModel != null && pageModel.Take == 0) pageModel.Take = 20;

            var queryUri = UriFactory.CreateDocumentCollectionUri("RogueSound", RoomConstants.Collection);

            var options = style == 0
                    ? new FeedOptions { EnableCrossPartitionQuery = true}
                    : null;

            var query = this.documentClient.CreateDocumentQuery<RoomModel>(queryUri, options);

            var filteredQuery = string.IsNullOrEmpty(searchName)
                ? query
                : query.Where(x => x.Name.Contains(searchName));

            var docQuery = filteredQuery.AddPaging(pageModel)
                    .AsDocumentQuery();


            var result = new List<RoomListResponseModel>();

            while (docQuery.HasMoreResults) result.AddRange(await docQuery.ExecuteNextAsync<RoomListResponseModel>());

            return new OkObjectResult(result);
          
        }
    }
}
