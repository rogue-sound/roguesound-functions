using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Common.Models
{
    public class RoomResponseModel
    {
        public Guid PublicId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Private { get; set; }

        public string Logo { get; set; }

        public string Cover { get; set; }

        public IEnumerable<string> Members { get; set; }
    }
}
