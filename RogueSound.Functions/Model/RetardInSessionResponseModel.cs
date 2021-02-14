using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Functions
{
    public class RetardInSessionResponseModel
    {
        public RetardInSessionResponseModel(string displayName, string avatar)
        {
            DisplayName = displayName;
            Avatar = avatar;
        }

        public string DisplayName { get; set; }
        public string Avatar { get; set; }
    }
}
