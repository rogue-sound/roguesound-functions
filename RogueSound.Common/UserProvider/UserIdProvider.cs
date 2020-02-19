using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSound.Common.UserProvider
{
    public static class UserIdProvider
    {
        public static string GetUserId(this HttpContext context) => context.Request.Headers["Authorization"];
    }
}
