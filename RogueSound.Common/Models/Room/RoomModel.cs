﻿using RogueSound.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Common.Models
{
    public class RoomModel
    {
        public string id { get; set; }

        public Guid PublicId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Creator { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Private { get; set; }

        public string Password { get; set; }

        public string Logo { get; set; }

        public string Cover { get; set; }

        public SessionConfiguration DefaultConfig { get; set; }

        public string Style { get; set; } = RoomConstants.RoomPartitionKey;
    }
}