using Microsoft.AspNetCore.Http;

namespace RogueSound.Common.UserProvider
{
    public static class UserIdProvider
    {
        public static string GetUserId(this HttpContext context) => context.Request.Headers["Authorization"];
    }
}
