using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Common.Sorting
{
    public static class HttpRequestExtensions
    {
        public static SortModel ExtractSorting(this HttpRequest request)
        {
            var propertyValue = request.Query["sortProperty"];
            var directionValue = request.Query["sortDirection"];

            var property = (propertyValue.Count > 0) ? propertyValue[0]: string.Empty;
            _ = int.TryParse(directionValue, out var sortDirection);

            return new SortModel
            {
                Property = property,
                SortDirection = (SortDirection)sortDirection
            };
        }
    }
}
